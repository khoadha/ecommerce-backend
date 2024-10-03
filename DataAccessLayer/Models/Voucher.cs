namespace DataAccessLayer.Models {
    public class Voucher : BaseEntity {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDisplay { get; set; }
        public int Percentage { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal MaxValue { get; set; }
        public int AvailableCount { get; set; }
    }
}
