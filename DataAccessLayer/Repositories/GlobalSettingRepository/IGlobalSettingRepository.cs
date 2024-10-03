using DataAccessLayer.BusinessModels;
namespace DataAccessLayer.Repositories.GlobalSettingRepository {
    public interface IGlobalSettingRepository {
        public Task<GlobalSetting> GetGlobalSettings();
        public Task UpdateBanner(List<string> list1,  List<string> list2);
        public Task<List<string>> GetBannerCarouselImages();
        public Task<List<string>> GetBannerImages();
    }
}
