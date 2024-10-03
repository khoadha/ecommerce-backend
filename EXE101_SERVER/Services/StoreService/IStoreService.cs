using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace EXE_API.Services.StoreService
{
    public interface IStoreService {
        Task<ServiceResponse<List<Store>>> GetStores();
        Task<ServiceResponse<List<Store>>> GetStoresForAdmin();
        Task<ServiceResponse<Store>> GetStoreById(int id);
        Task<ServiceResponse<List<Store>>> GetStoresByName(string name);
        Task<ServiceResponse<Store>> AddStore(Store store);
        Task<ServiceResponse<UpdateStoreDto>> UpdateStore(int id, UpdateStoreDto updatedStore);
        Task<ServiceResponse<Store>> DeleteStore(int id);
        Task<ServiceResponse<Store>> GetStoreByUserId(string userId);
        Task<ServiceResponse<Store>> UpdateVerifyState(int storeId, bool state);
        Task<ServiceResponse<Store>> UpdateBanState(int storeId, bool state);
        Task<ServiceResponse<Store>> UpdateStoreImage(int id, string imageUrlResponse);
        Task<ServiceResponse<Store>> UpdateStoreThumbnailImage(int id, string imageUrlResponse);
        Task<ServiceResponse<Store>> UpdateOpenState(int storeId, bool state);
        Task<ServiceResponse<bool>> IsStoreExist(string name);
        Task<ServiceResponse<StoreOverviewStatistic>> GetStoreOverviewStatistic(int id);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
