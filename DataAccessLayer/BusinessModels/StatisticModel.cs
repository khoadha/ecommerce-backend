using DataAccessLayer.Models;

namespace DataAccessLayer.BusinessModels {
    public class Statistic {
        public int StoreId { get; set; }
        public decimal Revenue { get; set; }
        public decimal ShippingCost { get; set; }
        public int OrderCount { get; set; }
    }

    public class OrderCountStatistic {
        public int CompletedCount { get; set; }
        public int CanceledCount { get; set; }
        public int ProcessingCount { get; set; }
        public int ConfirmedCount { get; set; }
    }

    public class AdminDashboardInformation {
        public decimal TotalPlatformRevenue { get; set; }
        public int TotalProductCount { get; set; }
        public int TotalShopCount { get; set; }
        public int TotalOrderCount { get; set; }
    }

    public class TopProductStatistic {
        public List<Product> TopProductsByViewCount { get; set; }
        public List<Product> TopProductsBySale { get; set; }
    }

    public class DailyRevenue {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ProductRatingData {
        public int AverageRating { get; set; }
        public int TotalRating { get; set; }
    }

    public class StoreOverviewStatistic {
        public int TotalProductCount { get; set; }
        public int TotalProductRatingCount { get; set; }
    }
}
