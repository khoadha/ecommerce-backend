using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.VoucherRepository {
    public class VoucherRepository : IVoucherRepository {

        private readonly EXEContext _context;
        public VoucherRepository(EXEContext context) {
            _context = context;
        }
        public async Task<List<Voucher>> GetVouchers() {
            var listVouchers = await _context.Vouchers.Where(a => a.IsDisplay == true).ToListAsync();
            return listVouchers;
        }

        public async Task<List<Voucher>> GetVouchersForAdmin() {
            var listVouchers = await _context.Vouchers.ToListAsync();
            return listVouchers;
        }

        public async Task<Voucher> GetVoucherById(int id) {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(a => a.Id == id);
            return voucher;
        }

        public async Task<Voucher> AddVoucher(Voucher newVoucher) {
            try {
                await _context.Vouchers.AddAsync(newVoucher);
                await SaveAsync();
                return newVoucher;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<Voucher> DeleteVoucher(int id) {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(a => a.Id == id);
            var response = voucher;
            if (voucher != null) {
                _context.Vouchers.Remove(voucher);
                await SaveAsync();
            }
            return response;
        }

        public async Task<Voucher> UpdateDisplayState(int VoucherId) {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(a => a.Id == VoucherId);
            if (voucher != null) {
                if (voucher.IsDisplay == true) {
                    voucher.IsDisplay = false;
                } else {
                    voucher.IsDisplay = true;
                }
                _context.Vouchers.Update(voucher);
                await SaveAsync();
            }
            return voucher;
        }

        public async Task<CheckAvailableVoucherResponseDto> CheckIsVoucherAvailableToUse(string code, double orderTotalPrice) {
            var result = new CheckAvailableVoucherResponseDto();
            try {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(a => a.Name.Equals(code));
                if (voucher != null) {
                    result.IsAvailable = CheckIsVoucherAvailableToUse(voucher, orderTotalPrice);
                    result.DiscountValue = CalculateDiscountValue(voucher, orderTotalPrice);
                    return result;
                }
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        private static double CalculateDiscountValue(Voucher voucher, double originalAmount) {
            double discount = originalAmount * (voucher.Percentage / 100.0);
            if (discount > (double)voucher.MaxValue) {
                discount = (double)voucher.MaxValue;
            }
            //double finalAmount = originalAmount - discount;
            return discount;
        }

        private static bool CheckIsVoucherAvailableToUse(Voucher voucher, double orderTotalPrice) {

            if (voucher.IsDisplay == false) {
                return false;
            }

            if (voucher.AvailableCount < 1) {
                return false;
            }

            if(voucher.ApprovedValue > (decimal)orderTotalPrice) {
                return false;
            }

            return true;
        }

        
    }
}
