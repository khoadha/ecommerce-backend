using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.PaymentTransactionRepository {
    public interface IPaymentTransactionRepository {
        void HandlePaymentSuccess(string txnRef);
        Task<List<PaymentTransaction>> GetPaymentTransactions(int? count);
        Task<PaymentTransaction> AddPaymentTransaction(PaymentTransaction pt);
        Task<bool> SaveAsync();
    }
}
