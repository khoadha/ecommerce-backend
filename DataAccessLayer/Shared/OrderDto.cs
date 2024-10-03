using DataAccessLayer.Models;

namespace DataAccessLayer.Shared {
    public class AddOrderDto {
        public int StoreId { get; set; }//
        public string? CustomerId { get; set; }//
        public string? PhoneNumber { get; set; }
        public bool IsPaid { get; set; }
        public string UserAddress { get; set; }//
        public decimal TotalCost { get; set; }//
        public decimal ShippingCost { get; set; }//
        public string ShipOption { get; set; }
        public int? PointToPayment { get; set; }
        public string? AppliedVoucherCode { get; set; }
        public ICollection<AddOrderProductsDto>? OrderItems { get; set; }
    }

    public class GetOrderDto {
        public int Id { get; set; }
        public string UserAddress { get; set; }//==============
        public DateTime OrderDate { get; set; } //==============
        public DateTime? ShipDate { get; set; } //==============
        public decimal ShippingCost { get; set; } //==============
        public decimal TotalCost { get; set; } //==============
        public bool IsPaid { get; set; }
        public OrderStatus Status { get; set; } //==============
        public string? CustomerId { get; set; } //==============
        public string? StoreName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CustomerName { get; set; }
        public ICollection<GetOrderProductsDto>? OrderItems { get; set; }
    }

    public class GetOrderByUserDto {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } //==============
        public DateTime? ShipDate { get; set; } //==============
        public bool IsPaid { get; set; }
        public decimal TotalCost { get; set; } //==============
        public OrderStatus Status { get; set; } //==============
        public string? StoreName { get; set; }
    }

    public class GetOrderByStoreDto {
        public int Id { get; set; }
        public string UserAddress { get; set; }//==============
        public DateTime OrderDate { get; set; } //==============
        public DateTime? ShipDate { get; set; } //==============
        public decimal ShippingCost { get; set; } //==============
        public decimal TotalCost { get; set; } //==============
        public bool IsPaid { get; set; }
        public OrderStatus Status { get; set; } //==============
        public string? CustomerId { get; set; } //==============
        public string? CustomerName { get; set; }
    }

    public class AddOrderProductsDto {
        public int ProductId { get; set; }//
        public int Quantity { get; set; }//
        public decimal Cost { get; set; }//
    }

    public class GetOrderProductsDto {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductImagePath { get; set; }
        public int Quantity { get; set; }//
        public decimal Cost { get; set; }//
    }
}
