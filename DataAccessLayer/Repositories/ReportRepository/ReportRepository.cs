using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.ReportRepository {
    public class ReportRepository : IReportRepository {

        private readonly EXEContext _context;

        public ReportRepository(EXEContext context) {
            _context = context;
        }

        public async Task AddReport(Report report) {
            try {
                if (report is not null) {
                    report.CreatedDate = DateTime.Now;
                    report.IsHandled = false;
                    await _context.Reports.AddAsync(report);
                    await SaveAsync();
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<List<Report>> GetReports(int? count) {
            try {
                if (count.HasValue) {
                    return await _context.Reports.Take(count.Value).ToListAsync();
                }
                return await _context.Reports.ToListAsync();
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<Report> GetReportById(int reportId) {
            try {
                var report = await _context.Reports.FirstOrDefaultAsync(a => a.Id == reportId);
                if (report is not null) {
                    return report;
                }
                return null;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task CompleteReportStatus(int reportId, string staffId) {
            try {
                var report = await _context.Reports.FirstOrDefaultAsync(a => a.Id == reportId);
                if (report is not null) {
                    report.IsHandled = true;
                    report.HandleStaffId = staffId;
                    _context.Reports.Update(report);
                    await SaveAsync();
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
