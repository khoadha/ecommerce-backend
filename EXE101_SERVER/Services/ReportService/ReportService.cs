using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.ReportRepository;

namespace EXE101_API.Services.ReportService {
    public class ReportService : IReportService {

        private readonly IReportRepository _repo;

        public ReportService(IReportRepository reportRepo)
        {
            _repo = reportRepo;
        }

        public async Task AddReport(Report report) {
            try {
                await _repo.AddReport(report);
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task CompleteReportStatus(int reportId, string staffId) {
            try {
                await _repo.CompleteReportStatus(reportId, staffId);
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<ServiceResponse<Report>> GetReportById(int reportId) {
            var serviceResponse = new ServiceResponse<Report>();
            try {
                var rp = await _repo.GetReportById(reportId);
                serviceResponse.Data = rp;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Report>>> GetReports(int? count) {
            var serviceResponse = new ServiceResponse<List<Report>>();
            try {
                var rp = await _repo.GetReports(count);
                serviceResponse.Data = rp;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
