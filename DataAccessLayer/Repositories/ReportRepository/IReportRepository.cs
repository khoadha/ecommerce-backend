using DataAccessLayer.Models;
namespace DataAccessLayer.Repositories.ReportRepository {
    public interface IReportRepository {
        Task AddReport(Report report);
        Task<List<Report>> GetReports(int? count);
        Task<Report> GetReportById(int reportId);
        Task CompleteReportStatus(int reportId, string staffId);
    }
}
