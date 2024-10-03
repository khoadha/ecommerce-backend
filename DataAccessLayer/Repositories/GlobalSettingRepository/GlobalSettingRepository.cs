using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Helper;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Repositories.GlobalSettingRepository {
    public class GlobalSettingRepository : IGlobalSettingRepository {

        private readonly EXEContext _context;

        public GlobalSettingRepository(EXEContext context)
        {
            _context = context;
        }

        public async Task<GlobalSetting> GetGlobalSettings() {
            var result = new GlobalSetting();
            try {
                await _context.GlobalSettings.ForEachAsync(x => {
                    if (x.SettingKey is not null && x.SettingValue is not null) {

                        if (x.SettingKey.Equals(GlobalSettingKey.AllowFirstTopupBonus)) {
                            result.AllowFirstTopupBonus = (bool)x.SettingValue.Convert(x.SettingType);
                        }
                        if (x.SettingKey.Equals(GlobalSettingKey.PlatformFeePercent)) {
                            result.PlatformFeePercent = (double)x.SettingValue.Convert(x.SettingType);
                        }
                        if (x.SettingKey.Equals(GlobalSettingKey.FirstTopupBonus)) {
                            result.FirstTopupBonus = (int)x.SettingValue.Convert(x.SettingType);
                        }
                    }
                });
                return result;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task UpdateBanner(List<string> listCarouselImages, List<string> listImages) {
            await _context.GlobalSettings.ForEachAsync(x => {
                if (x.SettingKey is not null && x.SettingValue is not null) {
                    if (x.SettingKey.Equals(GlobalSettingKey.BannerCarouselImages)) {
                        x.SettingValue = listCarouselImages.ConvertListToString();
                    }
                    if (x.SettingKey.Equals(GlobalSettingKey.BannerImages)) {
                        x.SettingValue = listImages.ConvertListToString();
                    }
                }
            });
            await _context.SaveChangesAsync();
        }

         public async Task<List<string>> GetBannerCarouselImages() {
            var result = new List<string>();
            await _context.GlobalSettings.ForEachAsync(x => {
                if (x.SettingKey is not null && x.SettingValue is not null) {

                    if (x.SettingKey.Equals(GlobalSettingKey.BannerCarouselImages)) {
                        result = x.SettingValue.ConvertStringToList();
;                    }
                }
            });
            return result;
        }

        public async Task<List<string>> GetBannerImages() {
            var result = new List<string>();
            await _context.GlobalSettings.ForEachAsync(x => {
                if (x.SettingKey is not null && x.SettingValue is not null) {

                    if (x.SettingKey.Equals(GlobalSettingKey.BannerImages)) {
                        result = x.SettingValue.ConvertStringToList();
                    }
                }
            });
            return result;
        }
    }
}
