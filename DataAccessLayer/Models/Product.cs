using System.ComponentModel.DataAnnotations;
namespace DataAccessLayer.Models {
    public class Product : BaseEntity {
        public string Name { get; set; } = string.Empty;
        public string? ImgPath { get; set; } 
        public decimal? Price { get; set; }
        public int NumberOfSales { get; set; }
        public int ViewCount { get; set; }
        public bool IsSoldOut { get; set; }
        public string? Description { get; set; }
        public int StoreId { get; set; }
        public Store? Store { get; set; }
        public ICollection<ProductImage>? ListImages { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
        public ICollection<ProductMaterial>? ProductMaterials { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }
    }

    public class ProductImage : BaseEntity {
        public string? Url { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }

    public class Feedback : BaseEntity {
        public string? Description { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public ICollection<FeedbackImage>? FeedbackImages { get; set; }

    }

    public class FeedbackImage : BaseEntity {
        public string? Url { get; set; }
        public Feedback? Feedback { get; set; }
    }

    public class ProductCategory : BaseEntity {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }

    public class ProductMaterial : BaseEntity {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
    }

    public class Category : BaseEntity {
        public string Name { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
        public ICollection<BiddingCategory>? BiddingCategories { get; set; }
    }

    public class Material : BaseEntity {
        public string Name { get; set; }
        public ICollection<ProductMaterial>? ProductMaterials { get; set; }
        public ICollection<BiddingMaterial>? BiddingMaterials { get; set; }
    }
}
