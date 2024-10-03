using DataAccessLayer.Enums;
namespace DataAccessLayer.Models {
    public class Report : BaseEntity {
        public string? Email { get; set; }
        public ReportTypeEnum ReportType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string? HandleStaffId { get; set; }
        public bool IsHandled { get; set; }
    }
}
