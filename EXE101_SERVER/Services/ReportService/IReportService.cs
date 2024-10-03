using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace EXE101_API.Services.ReportService {
    public interface IReportService {
        Task AddReport(Report report);
        Task<ServiceResponse<List<Report>>> GetReports(int? count);
        Task<ServiceResponse<Report>> GetReportById(int reportId);
        Task CompleteReportStatus(int reportId, string staffId);
    }
}
