using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.GlobalSettingRepository;

namespace EXE101_API.Services.GlobalSettingService {
    public class GlobalSettingService : IGlobalSettingService {

        private readonly IGlobalSettingRepository _repository;

        public GlobalSettingService(IGlobalSettingRepository repository) {
            _repository = repository;
        }

        public async Task<ServiceResponse<GlobalSetting>> GetGlobalSetting() {
            var serviceResponse = new ServiceResponse<GlobalSetting>();
            try {
                var st = await _repository.GetGlobalSettings();
                serviceResponse.Data = st;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task UpdateBanner(List<string> listCarouselImages, List<string> listImages) {
            try {
                await _repository.UpdateBanner(listCarouselImages, listImages);
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<ServiceResponse<List<string>>> GetBannerCarouselImages() {
            var serviceResponse = new ServiceResponse<List<string>>();
            try {
                var st = await _repository.GetBannerCarouselImages();
                serviceResponse.Data = st;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<string>>> GetBannerImages() {
            var serviceResponse = new ServiceResponse<List<string>>();
            try {
                var st = await _repository.GetBannerImages();
                serviceResponse.Data = st;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
