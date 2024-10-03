using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Shared;

namespace EXE_API.Services.StatisticService
{
    public class StatisticService : IStatisticService {

        private readonly IStoreRepository _storeRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public StatisticService(IStoreRepository storeRepo, IOrderRepository orderRepo, IMapper mapper) {
            _storeRepo = storeRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<OrderCountStatistic>> GetOrderCountStatistic(int storeId) {
            var serviceResponse = new ServiceResponse<OrderCountStatistic>();
            try {
                var store = await _storeRepo.GetStoreById(storeId);
                var response = await _orderRepo.GetOrderCountStatistic(store);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<DailyRevenue>>> GetRevenueFromLastMonth(int storeId) {
            var serviceResponse = new ServiceResponse<List<DailyRevenue>>();
            try {
                var store = await _storeRepo.GetStoreById(storeId);
                var response = await _orderRepo.GetRevenueFromLastMonth(store);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<TopProductStatisticDto>> GetTopProductStatistic(int storeId) {
            var serviceResponse = new ServiceResponse<TopProductStatisticDto>();
            try {
                var store = await _storeRepo.GetStoreById(storeId);
                var response = await _orderRepo.GetTopProductStatistic(store);

                var result = _mapper.Map<TopProductStatisticDto>(response);
                serviceResponse.Data = result;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Statistic>> GetStatistic(int storeId, DateTime? fromDate, DateTime? toDate) {
            var serviceResponse = new ServiceResponse<Statistic>();
            try {
                var store = await _storeRepo.GetStoreById(storeId);
                var response = await _orderRepo.GetStatistic(store, fromDate, toDate);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<OrderCountStatistic>> GetOrderCountStatistic() {
            var serviceResponse = new ServiceResponse<OrderCountStatistic>();
            try {
                var response = await _orderRepo.GetOrderCountStatistic();
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<DailyRevenue>>> GetRevenueFromLastMonth() {
            var serviceResponse = new ServiceResponse<List<DailyRevenue>>();
            try {
                var response = await _orderRepo.GetRevenueFromLastMonth();
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public ServiceResponse<AdminDashboardInformation> GetAdminDashboardInformation() {
            var serviceResponse = new ServiceResponse<AdminDashboardInformation>();
            try {
                var response = _orderRepo.GetAdminDashboardInformation();
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
