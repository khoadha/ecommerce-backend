using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
namespace DataAccessLayer.Repositories {
    public interface IStoreRepository {
        Task<List<Store>> GetStores();
        Task<List<Store>> GetStoresForAdmin();
        Task<Store> GetStoreById(int id);
        Task<Store> GetStoreByUserId(string userId);
        Task<List<Store>> GetStoresByName(string name);
        Task<StoreOverviewStatistic> GetStoreOverviewStatistic(int id);
        Task<Store> AddStore(Store store);
        Task<UpdateStoreDto> UpdateStore(int id, UpdateStoreDto updatedStore);
        Task<Store> UpdateStoreImage(int id, string imageUrlResponse);
        Task<Store> UpdateStoreThumbnailImage(int id, string imageUrlResponse);
        Task<Store> UpdateVerifyState(int storeId, bool state);
        Task<Store> UpdateBanState(int storeId, bool state);
        Task<Store> UpdateOpenState(int storeId, bool state);
        Task<Store> DeleteStore(int id);
        Task<bool> IsStoreExist(string name);
        Task<bool> SaveAsync();
    }
}
