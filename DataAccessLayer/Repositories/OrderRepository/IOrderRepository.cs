using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories {
    public interface IOrderRepository {
        Task<List<Order>> GetOrdersByUserId(string id);
        Task<List<Order>> GetOrdersByStoreId(int id);
        Task<Order> GetOrderById(int id);
        Task<Order> UpdateOrderState(int id, OrderStatus state);
        Task<bool> IsUserFirstOrdered(string userId);
        Task<Order> AddOrder(Order order, string shipOption, int? pointToPayment, string? appliedVoucherCode);
        Task<Statistic> GetStatistic(Store store, DateTime? fromDate, DateTime? toDate);
        Task<List<DailyRevenue>> GetRevenueFromLastMonth(Store store);
        Task<OrderCountStatistic> GetOrderCountStatistic(Store store);
        Task<TopProductStatistic> GetTopProductStatistic(Store store);
        Task<OrderCountStatistic> GetOrderCountStatistic();
        Task<List<DailyRevenue>> GetRevenueFromLastMonth();
        AdminDashboardInformation GetAdminDashboardInformation();
        Task<bool> SaveAsync();
    }
}
