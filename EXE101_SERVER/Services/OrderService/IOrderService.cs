using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace EXE_API.Services.OrderService
{
    public interface IOrderService {
        Task<ServiceResponse<List<Order>>> GetOrdersByUserId(string id);
        Task<ServiceResponse<List<Order>>> GetOrdersByStoreId(int id);
        Task<ServiceResponse<Order>> GetOrderById(int id);
        Task<ServiceResponse<Order>> UpdateOrderState(int id, OrderStatus state);
        Task<ServiceResponse<bool>> IsUserFirstOrdered(string userId);
        Task<ServiceResponse<Order>> AddOrder(Order order, string shipOption, int? pointToPayment, string? appliedVoucherCode);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
