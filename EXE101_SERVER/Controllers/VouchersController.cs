using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_API.Context;
using EXE101_API.Services.VoucherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EXE101_API.Controllers
{

    [ApiController]
    [Route("api/v1/")]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public VouchersController(IVoucherService voucherService, IMapper mapper, IUserContext userContext)
        {
            _voucherService = voucherService;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet]
        [Route("voucher")]
        public async Task<ActionResult<ServiceResponse<List<GetVoucherDto>>>> GetAllVouchers()
        {
            var listVoucherFromDb = await _voucherService.GetVouchers();
            var response = _mapper.Map<List<GetVoucherDto>>(listVoucherFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("voucher/admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<List<GetVoucherDto>>>> GetAllVouchersForAdmin()
        {
            var listVoucherFromDb = await _voucherService.GetVouchersForAdmin();
            var response = _mapper.Map<List<GetVoucherDto>>(listVoucherFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("voucher/{id}")]
        public async Task<ActionResult<ServiceResponse<GetVoucherDto>>> GetVoucherById([FromRoute] int id)
        {
            var voucherFromDb = await _voucherService.GetVoucherById(id);
            if (voucherFromDb.Data == null)
            {
                return NotFound($"voucher with id {id} is not existed!");
            }
            var response = _mapper.Map<GetVoucherDto>(voucherFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("voucher/check-available")]
        public async Task<IActionResult> CheckIsVoucherAvailableToUse([FromQuery] string code, [FromQuery] double orderTotalPrice) {

            //var currentUser = _userContext.GetCurrentUser(HttpContext);
            var response = await _voucherService.CheckIsVoucherAvailableToUse(code, orderTotalPrice);
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("voucher")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetVoucherDto>>> AddVoucher([FromBody] AddVoucherDto dto)
        {

            var voucher = _mapper.Map<Voucher>(dto);
            voucher.CreatedAt = DateTime.UtcNow.AddHours(7);
            voucher.IsDisplay = true;

            var voucherFromDb = await _voucherService.AddVoucher(voucher);

            var response = _mapper.Map<GetVoucherDto>(voucherFromDb.Data);
            return Ok(response);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut]
        [Route("voucher/display-state/{voucherId}")]
        public async Task<IActionResult> UpdateDisplayState([FromRoute] int voucherId)
        {
            var voucher = await _voucherService.UpdateDisplayState(voucherId);
            var response = _mapper.Map<GetVoucherDto>(voucher.Data);
            return Ok(response);
        }

        [HttpDelete]
        [Route("voucher/{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetVoucherDto>>> DeleteVoucher([FromRoute] int id)
        {
            var voucherFromDb = await _voucherService.GetVoucherById(id);
            if (voucherFromDb.Data == null)
            {
                return NotFound($"voucher with id {id} is not existed!");
            }
            var deletedItem = await _voucherService.DeleteVoucher(id);
            var response = _mapper.Map<GetVoucherDto>(deletedItem.Data);
            return Ok(response);
        }
    }
}
