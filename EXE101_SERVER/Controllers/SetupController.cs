using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE_API.Services.ApplicationUserService;
using EXE_API.Services.StatisticService;
using EXE101_API.Services.GlobalSettingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/setup/")]
    [Authorize(Roles = AppRole.ADMIN)]
    public class SetupController : ControllerBase
    {
        private readonly IApplicationUserService _userService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IStatisticService _statisticService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public SetupController(
            IStatisticService statisticService,
            IBlobService blobService,
            IGlobalSettingService globalSettingService,
            IMapper mapper,
            IApplicationUserService userService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<SetupController> logger)
        {
            _globalSettingService = globalSettingService;
            _mapper = mapper;
            _userService = userService;
            _blobService = blobService;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _statisticService = statisticService;
        }

        [HttpGet("global-settings")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGlobalSettings() {
            var result = await _globalSettingService.GetGlobalSetting();
            return Ok(result.Data);
        }

        [HttpGet("carousel-images")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBannerImages() {
            var banner = await _globalSettingService.GetBannerImages();
            var bannerCarousel = await _globalSettingService.GetBannerCarouselImages();
            return Ok(new { bannerImages = banner.Data, bannerCarouselImages = bannerCarousel.Data });
        }

        [HttpPut("carousel-images")]
        public async Task<IActionResult> UpdateBannerImages(UpdateBannerImagesDto dto) {
            await _globalSettingService.UpdateBanner(dto.BannerCarouselImages, dto.BannerImages);
            return NoContent();
        }

        [HttpGet("dashboard-information")]
        [Authorize(Roles = AppRole.ADMIN)]
        public IActionResult GetAdminDashboardInformation() {
            var result = _statisticService.GetAdminDashboardInformation();
            return Ok(result.Data);
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageLink([FromForm] GetImageLinkDto dto) {
            var url = await _blobService.UploadFileAsync(dto.File);
            return Ok(new { url });
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<ServiceResponse<List<ApplicationUserDto>>>> GetAllUsers()
        {
            var listUsersFromDb = await _userService.GetUsers();
            foreach (var user in listUsersFromDb.Data)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (user.ManagedStoreId != null)
                {
                    user.RoleName = "Manager";
                }
                else
                {
                    user.RoleName = role.First();
                }
            }
            var response = _mapper.Map<List<ApplicationUserDto>>(listUsersFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("roles/search")]
        public async Task<IActionResult> GetUserRoles([FromQuery] string email)
        {

            var user = await _userManager.FindByEmailAsync(email);


            if (user == null)
            {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return Ok(new
                {
                    result = $"The user with email {email} is not exist!"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The role {name} has been added successfully!");
                    return Ok(new
                    {
                        result = $"The role {name} has been added successfully!"
                    });
                }
                else
                {
                    _logger.LogInformation($"The role {name} has not been added!");
                    return BadRequest(new
                    {
                        error = $"The role {name} has not been added!"
                    });
                }
            }
            return BadRequest(new { error = "Role already existed" });
        }


        [HttpPost]
        [Route("roles/user/add")]
        public async Task<IActionResult> AddUserToRole([FromQuery] string email, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return BadRequest(new
                {
                    error = $"User does not exist!"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                _logger.LogInformation($"The role {roleName} is not exist!");
                return BadRequest(new
                {
                    error = $"Role does not exist!"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"The user with email {email} has been add to role {roleName} successfully!"
                });
            }
            else
            {
                _logger.LogInformation($"The user was not able to be added to the role!");
                return BadRequest(new
                {
                    error = $"The user was not able to be added to the role!"
                });
            }
        }

        [HttpPost]
        [Route("roles/user/remove")]
        public async Task<IActionResult> RemoveUserFromRole([FromQuery] string email, [FromQuery] string roleName)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return BadRequest(new
                {
                    error = $"User does not exist!"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The role {roleName} is not exist!");
                return BadRequest(new
                {
                    error = $"Role does not exist!"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"The user with email {email} has been remove from role {roleName} successfully!"
                });
            }
            else
            {
                _logger.LogInformation($"The user was not able to be removed from the role!");
                return BadRequest(new
                {
                    error = $"The user was not able to be removed from the role!"
                });
            }
        }

    }
}
