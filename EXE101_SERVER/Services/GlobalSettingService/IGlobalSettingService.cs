using DataAccessLayer.BusinessModels;

namespace EXE101_API.Services.GlobalSettingService {
    public interface IGlobalSettingService {
        public Task<ServiceResponse<GlobalSetting>> GetGlobalSetting();
        public Task UpdateBanner(List<string> listCarouselImages, List<string> listImages);
        public Task<ServiceResponse<List<string>>> GetBannerCarouselImages();
        public Task<ServiceResponse<List<string>>> GetBannerImages();
    }
}
