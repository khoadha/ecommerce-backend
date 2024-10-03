using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccessLayer.Models {
    public class Bidding : BaseEntity {
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
        [ForeignKey("CustomerId")]
        public string? CustomerId { get; set; }
        public ApplicationUser? Customer { get; set; }
        public ICollection<BiddingCategory>? BiddingCategories { get; set; }
        public ICollection<BiddingMaterial>? BiddingMaterials { get; set; }
        public virtual ICollection<Auctioneer> Auctioneers { get; set; } = new List<Auctioneer>();
    }

    public class BiddingCategory : BaseEntity {
        public int BiddingId { get; set; }
        public Bidding? Bidding { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }

    public class BiddingMaterial : BaseEntity {
        public int BiddingId { get; set; }
        public Bidding? Bidding { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
    }

    public class Auctioneer : BaseEntity {
        public DateTime? CompletedTime { get; set; }
        public double Price { get; set; }
        [ForeignKey("BiddingId")]
        public int BiddingId { get; set; }
        public double PercentOfComplete { get; set; }
        public bool? IsChosen { get; set; }
        public virtual Bidding? Bidding { get; set; }
        [ForeignKey("StoreId")]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }
        public ICollection<AuctioneerImage>? ListImages { get; set; }
    }

    public class AuctioneerImage : BaseEntity {
        public string? Url { get; set; }
        public Auctioneer? Auctioneer { get; set; }
    }
}
