using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using EXE_API.Services.ApplicationUserService;
using EXE101_API.Context;
using EXE101_API.Services.VnPayService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers {
    [Route("api/v1/payment/")]
    [ApiController]
    public class PaymentController : ControllerBase {

        private readonly IVnPayService _vnPayService;
        private readonly IApplicationUserService _accountService;
        private readonly IUserContext _userContext;
        public PaymentController(
            IUserContext userContext,
            IApplicationUserService accountService,
            IVnPayService vnPayService) {
            _accountService = accountService;
            _vnPayService = vnPayService;
            _userContext = userContext;
        }

        [HttpGet("transactions")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int? count) {

            var data = await _vnPayService.GetPaymentTransactions(count);
            return Ok(data.Data);
        }

        [HttpPost("create/{userId}")]
        [Authorize]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequestModel paymentRequestModel, [FromRoute] string userId) {

            var requestUser = _userContext.GetCurrentUser(HttpContext);

            var host = Request.Headers.Referer;

            if (requestUser.UserId != userId) {
                return Forbid();
            }

            var paymentModel = new PaymentInformationModel {
                OrderType = "topup",
                Amount = paymentRequestModel.PaymentAmount*1000,
                OrderDescription = paymentRequestModel.Description,
                Name = "",
            };

            var url = _vnPayService.CreatePaymentUrl(paymentModel, userId, HttpContext, host);
            var response = new { url = url.Result };

            return Ok(response);
        }

        [HttpPut("payment-success/{txnRef}/{userId}")]
        [Authorize]
        public IActionResult HandlePaymentSuccess([FromRoute] string txnRef, [FromRoute] string userId) {
            //txnRef = transactionId
            var requestUser = _userContext.GetCurrentUser(HttpContext);

            if (requestUser.UserId != userId) {
                return Forbid();
            } else {
                _vnPayService.HandlePaymentSuccess(txnRef);
            }

            return NoContent();
        }

        [HttpGet("point")]
        [Authorize]
        public async Task<IActionResult> GetWemadePoint(string userId) {

            var requestUser = _userContext.GetCurrentUser(HttpContext);

            if(requestUser.UserId != userId) {
                return Forbid();
            }

            var response = await _accountService.GetWemadePoint(userId);

            return Ok(response.Data);
        }


    }
}

