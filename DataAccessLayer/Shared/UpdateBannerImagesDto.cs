using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Shared {
    public class UpdateBannerImagesDto {
        public List<string>? BannerCarouselImages { get; set; }
        public List<string>? BannerImages { get; set; }
    }
    
    public class GetImageLinkDto {
        public IFormFile? File { get; set; }
    }
}
