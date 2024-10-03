using Microsoft.AspNetCore.Http;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Shared {
    public class GetProductDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int NumberOfSales { get; set; }
        public bool IsSoldOut { get; set; }
        public int StoreId { get; set; }
        public string StoreImgPath { get; set; }
        public string StoreName { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; } = string.Empty;
    }

    public class GetProductDetailPageDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int NumberOfSales { get; set; }
        public bool IsSoldOut { get; set; }
        public int StoreId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; } = string.Empty;
        public string StoreImgPath { get; set; }
        public string StoreName { get; set; }
    }


    public class AddProductDto {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> MaterialIds { get; set; }
        public int StoreId { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class UpdateProductDto {
        public string Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
    }

    public class GetProductImageDto {
        public int Id { get; set; }
        public string ImgPath { get; set; }
    }

    public class SearchPayloadDto {
        public string? Search { get; set; }
        public int Size { get; set; }
        public int OffSet { get; set; }
        public SortEnum SortOption { get; set; }
        public int StarOption { get; set; }
        public IEnumerable<int>? Categories { get; set; }
        public IEnumerable<int>? Materials { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
    }

    public class SearchProductResponse {
        public IEnumerable<GetProductDto>? Data { get; set; }
        public int Total { get; set; }
    }

    public class UpdateProductImageDto {
        public IFormFile? Image { get; set; }
    }
}
