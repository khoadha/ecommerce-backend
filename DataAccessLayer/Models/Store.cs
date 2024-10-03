using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models {
    public class Store : BaseEntity {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public string ThumbnailImgPath { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool IsBanned { get; set; }
        public bool IsOpen { get; set; }
        public int DistrictId { get; set; }
        public int? BillingPackageId { get; set; }
        public bool IsPayPackageDeposit { get; set; }
        public DateTime? BillingPackageExpiredDate { get; set; }
        public string WardCode { get; set; } = string.Empty;
        public virtual ICollection<Product>? Products { get; set; }
        [NotMapped]
        public string? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }
        public virtual ICollection<Billing>? Billings { get; set; }
    }
}
