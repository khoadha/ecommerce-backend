using DataAccessLayer.Models;

namespace DataAccessLayer.Shared {
    public class TopProductStatisticDto {
        public List<ProductForTopStatisticDto> TopProductsByViewCount { get; set; }
        public List<ProductForTopStatisticDto> TopProductsBySale { get; set; }
    }

    public class ProductForTopStatisticDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImgPath { get; set; }
        public int NumberOfSales { get; set; }
        public int ViewCount { get; set; }
    }

}
