using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace EXE101_API.Services.VoucherService
{
    public interface IVoucherService {
        Task<ServiceResponse<List<Voucher>>> GetVouchers();
        Task<ServiceResponse<List<Voucher>>> GetVouchersForAdmin();
        Task<ServiceResponse<Voucher>> GetVoucherById(int id);
        Task<ServiceResponse<Voucher>> AddVoucher(Voucher voucher);
        Task<ServiceResponse<Voucher>> UpdateDisplayState(int voucherId);
        Task<ServiceResponse<Voucher>> DeleteVoucher(int id);
        Task<ServiceResponse<bool>> SaveAsync();
        Task<ServiceResponse<CheckAvailableVoucherResponseDto>> CheckIsVoucherAvailableToUse(string code, double orderTotalPrice);
    }
}
