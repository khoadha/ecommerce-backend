using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Shared {
    public class AddBiddingRequestDto {
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Quantity { get; set; }
        public decimal? Size { get; set; }
        public string? UnitOfMeasure { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? Note { get; set; }
        public string? CustomerId { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? MaterialIds { get; set; }
    }

    public class GetBiddingDto {
        public int Id { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Quantity { get; set; }
        public bool IsDone { get; set; }
        public string? ImageUrl { get; set; }
        public string? Note { get; set; }
        public string? CustomerId { get; set; }
    }

    public class GetBiddingDetailDto {
        public int Id { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Quantity { get; set; }
        public decimal? Size { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? ImageUrl { get; set; }
        public string? Note { get; set; }
        public bool? IsDone { get; set; }
        public bool? IsPaid { get; set; }
        public string? CustomerId { get; set; }
        public ICollection<string>? BiddingCategories { get; set; }
        public ICollection<string>? BiddingMaterials { get; set; }
    }

    public class GetAuctioneersByBiddingDto {
        public int Id { get; set; }
        public DateTime? CompletedTime { get; set; }
        public double Price { get; set; }
        public int BiddingId { get; set; }
        public double PercentOfComplete { get; set; }
        public bool? IsChosen { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? StoreImgPath { get; set; }
        public ICollection<string>? ListImages { get; set; }
    }

    public class AddAuctioneerDto {
        public DateTime? CompletedTime { get; set; }
        public double Price { get; set; }
        public int BiddingId { get; set; }
        public double PercentOfComplete { get; set; }
        public int StoreId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
