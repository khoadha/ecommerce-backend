using DataAccessLayer.Enums;
namespace DataAccessLayer.Models {
    public class Billing : BaseEntity {
        public int StoreId { get; set; }
        public double TotalBill { get; set; }
        public DateTime? CreatedDate { get; set; }
        public BillingStatus Status { get; set; }
        public Store? Store { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }

    public class BillingPackage : BaseEntity {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Percentage { get; set; }
        public double Price { get; set; }
    }
}
