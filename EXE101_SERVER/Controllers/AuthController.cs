using AutoMapper;
using DataAccessLayer.Models;
using EXE_API.Services.ApplicationUserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EXE_API.Services.CartService;
using EXE101_Backend.Shared;
using Google.Apis.Auth;
using EXE_API.Services.StoreService;
using EXE101_API.Services.EmailService;
using DataAccessLayer.Template;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using DataAccessLayer.Constants;
using DataAccessLayer.BusinessModels;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class AuthController : ControllerBase
    {

        #region Properties and Constructor
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicationUserService _userService;
        private readonly ICartService _cartService;
        private readonly IStoreService _storeService;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailSender;
        private static readonly IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        private readonly string googleClientId = config["Google:Credential:ClientId"];
        private readonly string jwtSecret = config["JWT:Secret"];
        private readonly string secureKey = config["Secure:Key"];
        private readonly TimeZoneInfo timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(config["TimeZoneId"]);

        public AuthController(
            IEmailService emailSender,
            IMapper mapper,
            IStoreService storeService,
            ICartService cartService,
            IApplicationUserService userService,
            IBlobService blobService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _cartService = cartService;
            _emailSender = emailSender;
            _userService = userService;
            _userManager = userManager;
            _blobService = blobService;
            _roleManager = roleManager;
            _storeService = storeService;
            _mapper = mapper;
        }
        #endregion

        #region Action
        [HttpPost]
        [Route("auth/register")]
        public async Task<IActionResult> Register([FromForm] UserRegistrationRequestDto model)
        {
            if (ModelState.IsValid)
            {

                var userExists = await _userManager.FindByEmailAsync(model.Email);

                var isPhoneNumberExist = await _userManager.Users.AnyAsync(a => a.PhoneNumber == model.PhoneNumber);

                if (userExists != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() {
                            "Email đã được sử dụng cho một tài khoản khác!"
                        }
                    });
                }

                if (isPhoneNumberExist)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() {
                            "Số điện thoại đã được sử dụng cho một tài khoản khác!"
                        }
                    });
                }


                Cart cart = new Cart();
                var newCart = await _cartService.AddCart(cart);
                var imageUrl = await _blobService.UploadFileAsync(model.Image);

                var newUser = new ApplicationUser() {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Cart = newCart.Data,
                    CartId = newCart.Data.Id,
                    ImgPath = imageUrl,
                    WemadePoint = 0,
                    IsFirstTopup = false
                };

                var isCreated = await _userManager.CreateAsync(newUser, model.Password);

                if (isCreated.Succeeded)
                {

                    await _userManager.AddToRoleAsync(newUser, AppRole.USER);

                    var token = await GenerateToken(newUser);

                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    confirmEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmEmailToken));

                    var confirmationLink = BuildEmailConfirmationLink(newUser.Id, confirmEmailToken, Request.Headers.Referer);

                    string encodedCallbackUrl = HtmlEncoder.Default.Encode(confirmationLink);

                    ConfirmEmailTemplate template = new();

                    var receiver = new string[] { newUser.Email };

                    string subject = template.Subject;

                    string htmlBody = ConfirmEmailTemplate.Get(encodedCallbackUrl);

                    var message = new Message(receiver, subject, htmlBody);

                    _emailSender.SendEmail(message);

                    return Ok(new { Result = true, email = newUser.Email });
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                            "Lỗi máy chủ"
                        },
                        Result = false
                    });
                }

            }
            return BadRequest();
        }

        [HttpGet]
        [Route("auth/environment-data")]
        public IActionResult GetEnvironmentData()
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(googleClientId);
            string encodedText = Convert.ToBase64String(bytesToEncode);
            var datas = new { secure_client = encodedText, secure_key = secureKey };
            return Ok(datas);
        }

        [HttpPost]
        [Route("auth/login-google")]
        public async Task<IActionResult> LoginWithGoogle(string credential)
        {

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { googleClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

                var existingUser = _userManager.Users.FirstOrDefault(a => a.NormalizedEmail == payload.Email.ToUpper());

                if (payload.EmailVerified == true)
                {
                    if (existingUser != null)
                    {

                        var data = await _userManager.GetLoginsAsync(existingUser);

                        var isLoginWithGoogle = data.Any(a => a.LoginProvider.Equals(OAuthProvider.GOOGLE));

                        if (!isLoginWithGoogle)
                        {
                            return BadRequest(new AuthResult()
                            {
                                Errors = new List<string>() { "Email này đã được sử dụng" },
                                Result = false
                            });
                        }


                        var newAccessToken = await GenerateToken(existingUser);
                        var newRefreshToken = await CreateRefreshToken();

                        existingUser.LastLoginTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                        existingUser.RefreshToken = newRefreshToken;
                        existingUser.RefreshTokenExpiryTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById).AddDays(30);
                        _userService.Save();
                        return Ok(new TokenApiDto
                        {
                            AccessToken = newAccessToken,
                            RefreshToken = newRefreshToken
                        });

                    }
                    else
                    {
                        Cart cart = new();
                        var newCart = await _cartService.AddCart(cart);
                        var newUser = new ApplicationUser()
                        {
                            UserName = payload.Name,
                            Email = payload.Email,
                            ImgPath = payload.Picture,
                            Cart = newCart.Data,
                            CartId = newCart.Data.Id,
                            WemadePoint = 0,
                            IsFirstTopup = false
                        };

                        var isCreated = await _userManager.CreateAsync(newUser);

                        if (isCreated.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(newUser, AppRole.USER);
                            var userLoginInfo = new UserLoginInfo(OAuthProvider.GOOGLE, payload.Subject, OAuthProvider.GOOGLE);
                            await _userManager.AddLoginAsync(newUser, userLoginInfo);
                            var newAccessToken = await GenerateToken(newUser);
                            var newRefreshToken = await CreateRefreshToken();
                            newUser.RefreshToken = newRefreshToken;
                            newUser.LastLoginTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                            newUser.RefreshTokenExpiryTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById).AddDays(30);
                            _userService.Save();

                            return Ok(new TokenApiDto
                            {
                                AccessToken = newAccessToken,
                                RefreshToken = newRefreshToken
                            });

                        }
                        else
                        {
                            return BadRequest(new AuthResult()
                            {
                                Errors = new List<string>() { "Server error" },
                                Result = false
                            });
                        }
                    }
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string> {
                        "Email chưa được xác minh!"
                },
                        Result = false
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { ex.Message },
                    Result = false
                });
            }
        }

        [HttpPost]
        [Route("auth/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto model)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string> {
                    "Tài khoản này không tồn tại, vui lòng thử lại!"
                },
                        Result = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);

                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string> {
                    "Sai mật khẩu, vui lòng thử lại!"
                },
                        Result = false
                    });
                }

                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(existingUser);

                if (!isEmailConfirmed)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string> {
                    "Email của tài khoản chưa được xác minh!"
                },
                        Result = false
                    });
                }

                existingUser.Token = await GenerateToken(existingUser);
                var newAccessToken = existingUser.Token;
                var newRefreshToken = await CreateRefreshToken();
                existingUser.RefreshToken = newRefreshToken;
                existingUser.LastLoginTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                existingUser.RefreshTokenExpiryTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                _userService.Save();

                return Ok(new TokenApiDto()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest(new AuthResult()
            {
                Errors = new List<string> {
                    "Thông tin không hợp lệ"
                },
                Result = false
            });
        }

        [HttpPost]
        [Route("auth/register-store")]
        public async Task<IActionResult> RegisterStore([FromForm] StoreRegistrationRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var storeImageUrl = await _blobService.UploadFileAsync(model.Image);
                var thumbImageUrl = await _blobService.UploadFileAsync(model.ThumbnailImage);
                var isStoreExist = await _storeService.IsStoreExist(model.Name);
                if (isStoreExist.Data)
                {
                    return Conflict("Cửa hàng với tên này đã tồn tại!");
                }
                var store = _mapper.Map<Store>(model);
                store.ManagerId = user.Id;
                store.IsBanned = false;
                store.IsVerified = false;
                store.IsOpen = false;
                store.ImgPath = storeImageUrl;
                store.ThumbnailImgPath = thumbImageUrl;
                store.BillingPackageExpiredDate = DateTime.Now;
                store.IsPayPackageDeposit = false;
                store.BillingPackageId = 0;
                await _storeService.AddStore(store);
                return Ok(store);
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() {
                            "Lỗi máy chủ"
                        },
                    Result = false
                });
            }
        }

        [HttpPost]
        [Route("auth/refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
            {
                return BadRequest("Yêu cầu không hợp lệ!");
            }
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);

            var email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await _userService.GetUserByEmail(email);

            if (user is null || user.Data.RefreshTokenExpiryTime <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById))
            {
                return BadRequest(new { message = "TokenExpired" }); // Do not change this line
            }

            var newAccessToken = await GenerateToken(user.Data);
            var newRefreshToken = await CreateRefreshToken();
            user.Data.RefreshToken = newRefreshToken;
            _userService.Save();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpGet("auth/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (userId == null || token == null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Đường dẫn đã hết hạn!"
                        }
                });
            }
            else if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Không tìm thấy người dùng!"
                        }
                });
            }
            else
            {

                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Cám ơn vì đã xác minh email!" });
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() {
                            "Email chưa được xác minh!"
                        }
                    });
                }
            }
        }

        [HttpPost]
        [Route("auth/forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string> {
                    "Thông tin không hợp lệ"
                },
                    Result = false
                });
            }

            var applicationUser = await _userManager.FindByEmailAsync(model.Email);

            if (applicationUser == null || !await _userManager.IsEmailConfirmedAsync(applicationUser))
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Không tìm thấy người dùng!"
                        }
                });
            }
            // Generate password reset token
            var confirmEmailToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            // Construct the password reset link

            confirmEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmEmailToken));

            var confirmationLink = BuildResetPasswordLink(applicationUser.Id, confirmEmailToken, Request.Headers.Referer);

            string encodedCallbackUrl = HtmlEncoder.Default.Encode(confirmationLink);

            // Send the password reset email

            ResetPasswordTemplate template = new();

            var receiver = new string[] { model.Email };

            string subject = template.Subject;

            string htmlBody = ResetPasswordTemplate.Get(encodedCallbackUrl);

            var message = new Message(receiver, subject, htmlBody);

            _emailSender.SendEmail(message);

            return Ok(new { Result = true, email = model.Email });
        }

        [HttpPost]
        [Route("auth/reset-password")]
        public async Task<IActionResult> ResetPassword(string userId, string token, [FromBody] string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string> {
                    "Thông tin không hợp lệ"
                },
                    Result = false
                });
            }
            if (userId == null || token == null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Đường dẫn đã hết hạn!"
                        }
                });
            }

            var applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Không tìm thấy người dùng!"
                        }
                });
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Reset password
            var result = await _userManager.ResetPasswordAsync(applicationUser, decodedToken, newPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "Đặt lại mật khẩu thành công!" });
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                            "Đặt lại mật khẩu thất bại"
                        }
                });
            }
        }

        #endregion

        #region Private Method
        private string BuildEmailConfirmationLink(string userId, string token, string callbackUrl)
        {
            return $"{callbackUrl}check-register-email?userId={userId}&token={token}";
        }
        private string BuildResetPasswordLink(string userId, string token, string callbackUrl)
        {
            return $"{callbackUrl}reset-password?userId={userId}&token={token}";
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById).AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        private async Task<List<Claim>> GetAllValidClaims(ApplicationUser user)
        {

            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id),
                    new Claim("ImgPath", user.ImgPath),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById).ToUniversalTime().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById).AddMinutes(15).ToUniversalTime().ToString())
                };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    if (user.ManagedStoreId != null)
                    {
                        claims.Add(new Claim("ManagedStoreId", user.ManagedStoreId.ToString()));
                    }
                    if (userRole == AppRole.USER)
                    {
                        if (!string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            claims.Add(new Claim("PhoneNumber", user.PhoneNumber));
                        }
                        if (user.CartId != null)
                        {
                            claims.Add(new Claim("CartId", user.CartId.ToString()));
                        }
                    }
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        private async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            var listOfUsers = await _userService.GetUsers();
            if (listOfUsers.Data != null)
            {
                var tokenInUser = listOfUsers.Data.Any(a => a.RefreshToken == refreshToken);
                if (tokenInUser)
                {
                    return await CreateRefreshToken();
                }
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is invalid token!");
            }
            return principal;
        }
        #endregion
    }
}

