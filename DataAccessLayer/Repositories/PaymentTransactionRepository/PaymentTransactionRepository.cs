
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories.PaymentTransactionRepository {
    public class PaymentTransactionRepository : IPaymentTransactionRepository {

        private readonly EXEContext _context;

        public PaymentTransactionRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactions(int? count) {
            if(count.HasValue) {
                return await _context.PaymentTransactions.OrderByDescending(a=>a.Id).Take(count.Value).ToListAsync();
            }
            return await _context.PaymentTransactions.ToListAsync();
        }

        public async Task<PaymentTransaction> AddPaymentTransaction(PaymentTransaction transaction) {
            try {
                var isExistTransaction = await _context.PaymentTransactions.AnyAsync(fc => fc.VnPayTransactionId == transaction.VnPayTransactionId);
                if (!isExistTransaction) {
                    await _context.PaymentTransactions.AddAsync(transaction);
                    await SaveAsync();
                } else {
                    throw new Exception("This transaction is existed.");
                }

            } catch (Exception ex) {
                throw;
            }
            return transaction;
        }

        public void HandlePaymentSuccess(string txnRef) {
            try {
                PaymentTransaction transaction = _context.PaymentTransactions.First(fc => fc.VnPayTransactionId == txnRef);
                if (transaction != null) {
                    var user = _context.Users.FirstOrDefault(a => a.Id == transaction.AccountId);
                    if (user is not null) {
                        if (transaction.TransactionStatus == TransactionStatus.Pending) {

                            user.WemadePoint += ((int)transaction.Amount / 1000);

                            _context.Users.Update(user);

                            transaction.TransactionStatus = TransactionStatus.Success;
                            _context.PaymentTransactions.Update(transaction);
                            _context.SaveChanges();
                        }
                    }
                } else {
                    throw new Exception("This transaction is not existed.");
                }
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
