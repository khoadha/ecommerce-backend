using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.VoucherRepository;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;

namespace EXE101_API.Services.VoucherService
{
    public class VoucherService : IVoucherService {

        private readonly IVoucherRepository _voucherRepo;

        public VoucherService(IVoucherRepository voucherRepo) {
            _voucherRepo = voucherRepo;
        }

        public async Task<ServiceResponse<Voucher>> AddVoucher(Voucher voucher) {
            var serviceResponse = new ServiceResponse<Voucher>();
            try {
                var addedVoucher = await _voucherRepo.AddVoucher(voucher);
                serviceResponse.Data = addedVoucher;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Voucher>> DeleteVoucher(int id) {
            var serviceResponse = new ServiceResponse<Voucher>();
            try {
                var deletedVoucher = await _voucherRepo.DeleteVoucher(id);
                if (deletedVoucher != null) {
                    serviceResponse.Data = deletedVoucher;
                }
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Voucher>> GetVoucherById(int id) {
            var serviceResponse = new ServiceResponse<Voucher>();
            try {
                var voucher = await _voucherRepo.GetVoucherById(id);
                serviceResponse.Data = voucher;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Voucher>>> GetVouchers() {
            var serviceResponse = new ServiceResponse<List<Voucher>>();
            try {
                var listVouchers = await _voucherRepo.GetVouchers();
                serviceResponse.Data = listVouchers;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Voucher>>> GetVouchersForAdmin() {
            var serviceResponse = new ServiceResponse<List<Voucher>>();
            try {
                var listVouchers = await _voucherRepo.GetVouchersForAdmin();
                serviceResponse.Data = listVouchers;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Voucher>> UpdateDisplayState(int voucherId) {
            var serviceResponse = new ServiceResponse<Voucher>();
            try {
                var response = await _voucherRepo.UpdateDisplayState(voucherId);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<bool>> SaveAsync() {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _voucherRepo.SaveAsync();
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CheckAvailableVoucherResponseDto>> CheckIsVoucherAvailableToUse(string code, double o) {
            var serviceResponse = new ServiceResponse<CheckAvailableVoucherResponseDto>();
            try {
                var response = await _voucherRepo.CheckIsVoucherAvailableToUse(code, o);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
