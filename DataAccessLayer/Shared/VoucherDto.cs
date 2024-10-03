using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared {
    public class GetVoucherDto {
        public int Id { get; set; }
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

    public class AddVoucherDto {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public int Percentage { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal MaxValue { get; set; }
        public int AvailableCount { get; set; }
    }

    public class CheckAvailableVoucherResponseDto {
        public bool? IsAvailable { get; set; }
        public double? DiscountValue { get; set; }
    }
}
