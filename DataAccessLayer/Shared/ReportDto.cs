using DataAccessLayer.Enums;

namespace DataAccessLayer.Shared {
    public class AddReportDto {
        public string? Email { get; set; }
        public ReportTypeEnum ReportType { get; set; }
        public string? Description { get; set; }
    }
}
