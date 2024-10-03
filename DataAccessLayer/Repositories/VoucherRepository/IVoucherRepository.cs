using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace DataAccessLayer.Repositories.VoucherRepository {
    public interface IVoucherRepository {
        Task<List<Voucher>> GetVouchers();
        Task<List<Voucher>> GetVouchersForAdmin();
        Task<Voucher> GetVoucherById(int id);
        Task<Voucher> AddVoucher(Voucher voucher);
        Task<Voucher> UpdateDisplayState(int voucherId);
        Task<Voucher> DeleteVoucher(int id);
        Task<bool> SaveAsync();
        Task<CheckAvailableVoucherResponseDto> CheckIsVoucherAvailableToUse(string code, double o);

    }
}
