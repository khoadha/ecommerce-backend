using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using DataAccessLayer.Shared;
using DataAccessLayer.Repositories;
using DataAccessLayer.BusinessModels;

namespace EXE_API.Services.StoreService
{
    public class StoreService : IStoreService {

        private readonly IStoreRepository _storeRepo;

        public StoreService(IStoreRepository storeRepo) {
            _storeRepo = storeRepo;
        }

        public async Task<ServiceResponse<List<Store>>> GetStores() {
            var serviceResponse = new ServiceResponse<List<Store>>();
            try {
                var listStores = await _storeRepo.GetStores();
                serviceResponse.Data = listStores;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Store>>> GetStoresForAdmin() {
            var serviceResponse = new ServiceResponse<List<Store>>();
            try {
                var listStores = await _storeRepo.GetStoresForAdmin();
                serviceResponse.Data = listStores;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> GetStoreById(int id) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var store = await _storeRepo.GetStoreById(id);
                serviceResponse.Data = store;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> GetStoreByUserId(string userId) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var managedStore = await _storeRepo.GetStoreByUserId(userId);
                serviceResponse.Data = managedStore;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Store>>> GetStoresByName(string name) {
            var serviceResponse = new ServiceResponse<List<Store>>();
            try {
                var stores = await _storeRepo.GetStoresByName(name);
                serviceResponse.Data = stores;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsStoreExist(string name) {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var result = await _storeRepo.IsStoreExist(name);
                serviceResponse.Data = result;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> AddStore(Store newStore) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var addedStore = await _storeRepo.AddStore(newStore);
                serviceResponse.Data = addedStore;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UpdateStoreDto>> UpdateStore(int id, UpdateStoreDto updatedStore) {
            var serviceResponse = new ServiceResponse<UpdateStoreDto>();
            try {
                var updatedStoreResponse = await _storeRepo.UpdateStore(id, updatedStore);
                serviceResponse.Data = updatedStoreResponse;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> DeleteStore(int id) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var deletedStore = await _storeRepo.DeleteStore(id);
                if (deletedStore != null) {
                    serviceResponse.Data = deletedStore;
                }
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SaveAsync() {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _storeRepo.SaveAsync();
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> UpdateVerifyState(int storeId, bool state) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var response = await _storeRepo.UpdateVerifyState(storeId, state);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> UpdateBanState(int storeId, bool state) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var response = await _storeRepo.UpdateBanState(storeId, state);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<Store>> UpdateOpenState(int storeId, bool state) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var response = await _storeRepo.UpdateOpenState(storeId, state);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> UpdateStoreImage(int id, string imageUrlResponse) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var response = await _storeRepo.UpdateStoreImage(id, imageUrlResponse);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Store>> UpdateStoreThumbnailImage(int id, string imageUrlResponse) {
            var serviceResponse = new ServiceResponse<Store>();
            try {
                var response = await _storeRepo.UpdateStoreThumbnailImage(id, imageUrlResponse);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<StoreOverviewStatistic>> GetStoreOverviewStatistic(int id) {
            var serviceResponse = new ServiceResponse<StoreOverviewStatistic>();
            try {
                var response = await _storeRepo.GetStoreOverviewStatistic(id);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
