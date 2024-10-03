namespace DataAccessLayer.BusinessModels {
    public class GlobalSetting {
        public double? PlatformFeePercent { get; set; }
        public int? FirstTopupBonus { get; set; }
        public bool AllowFirstTopupBonus { get; set; }
        public string? BannerCarouselImages { get; set; }
        public string? BannerImages { get; set; }
    }
}
