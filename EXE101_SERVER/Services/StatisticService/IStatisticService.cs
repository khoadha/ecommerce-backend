using DataAccessLayer.BusinessModels;
using DataAccessLayer.Shared;

namespace EXE_API.Services.StatisticService
{
    public interface IStatisticService {
        Task<ServiceResponse<Statistic>> GetStatistic(int storeId, DateTime? fromDate, DateTime? toDate);
        Task<ServiceResponse<TopProductStatisticDto>> GetTopProductStatistic(int storeId);
        Task<ServiceResponse<OrderCountStatistic>> GetOrderCountStatistic(int storeId);
        Task<ServiceResponse<List<DailyRevenue>>> GetRevenueFromLastMonth(int storeId);
        Task<ServiceResponse<OrderCountStatistic>> GetOrderCountStatistic();
        Task<ServiceResponse<List<DailyRevenue>>> GetRevenueFromLastMonth();
        ServiceResponse<AdminDashboardInformation> GetAdminDashboardInformation();

    }
}
