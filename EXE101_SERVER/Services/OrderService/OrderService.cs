using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EXE_API.Services.OrderService
{
    public class OrderService : IOrderService {

        public readonly IOrderRepository _orderRepo;
            
        public OrderService(IOrderRepository orderRepo) {
             _orderRepo = orderRepo;
        }

        public async Task<ServiceResponse<List<Order>>> GetOrdersByStoreId(int id) {
            var serviceResponse = new ServiceResponse<List<Order>>();
            try {
                var response = await _orderRepo.GetOrdersByStoreId(id);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Order>>> GetOrdersByUserId(string id) {
            var serviceResponse = new ServiceResponse<List<Order>>();
            try {
                var response = await _orderRepo.GetOrdersByUserId(id);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> GetOrderById(int id) {
            var serviceResponse = new ServiceResponse<Order>();
            try {
                var response = await _orderRepo.GetOrderById(id);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Order>> UpdateOrderState(int id, OrderStatus state) {
            var serviceResponse = new ServiceResponse<Order>();
            try {
                var response = await _orderRepo.UpdateOrderState(id,state);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<Order>> AddOrder(Order order, string shipOption, int? pointToPayment, string? appliedVoucherCode) {
            var serviceResponse = new ServiceResponse<Order>();
            try {
                var response = await _orderRepo.AddOrder(order, shipOption, pointToPayment, appliedVoucherCode);
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
                var response = await _orderRepo.SaveAsync();
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsUserFirstOrdered(string userId) {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _orderRepo.IsUserFirstOrdered(userId);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
