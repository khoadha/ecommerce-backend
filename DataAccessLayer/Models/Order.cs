using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models {
    public class Order : BaseEntity {
        public string UserAddress { get; set; } = string.Empty; //==============
        public DateTime OrderDate { get; set; } //==============
        public DateTime? ShipDate { get; set; } //==============
        public decimal ShippingCost { get; set; } //==============
        public decimal TotalCost { get; set; } //==============
        public OrderStatus Status { get; set; } //==============
        public bool IsPaid { get; set; }
        public bool IsAdminPaid { get; set; }
        public int? AppliedVoucherId { get; set; }
        public string? PhoneNumber { get; set; }
        [ForeignKey("CustomerId")]
        public string? CustomerId { get; set; } //==============
        public ApplicationUser? Customer { get; set; } //==============
        public int StoreId { get; set; } //==============

        [NotMapped]
        public string? StoreName {
            get { return Store?.Name; }
        }

        [NotMapped]
        public string? CustomerName {
            get { return Customer?.UserName; }
        }

        public Store? Store { get; set; } //==============
        public ICollection<OrderProducts>? OrderItems { get; set; }
    }   


    public class OrderProducts : BaseEntity {
        [ForeignKey("OrderId")]
        public int OrderId { get; set; } //==============
        public Order Order { get; set; } = null!;//==============
        [ForeignKey("ProductId")]
        public int ProductId { get; set; } //==============

        [NotMapped]
        public string? ProductName {
            get { return Product?.Name; }
        }

        [NotMapped]
        public decimal? ProductPrice {
            get { return Product?.Price; }
        }

        [NotMapped]
        public string? ProductImagePath {
            get { return Product?.ImgPath; }
        }

        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }

    public enum OrderStatus {
        Canceled = 1,
        Processing = 2,
        Confirmed = 3,
        Completed = 4
    }
}
