using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Repositories {
    public class StoreRepository : IStoreRepository {
        private readonly EXEContext _context;

        public StoreRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<Store>> GetStores() {
            var listStores = await _context.Stores.OrderByDescending(a=>a.Id).Where(a=>a.IsBanned==false).ToListAsync();
            return listStores;
        }

        public async Task<List<Store>> GetStoresForAdmin() {
            var listStores = await _context.Stores.Include(a=>a.Manager).ToListAsync();
            return listStores;
        }

        public async Task<Store> GetStoreById(int id) {
            var store = await _context.Stores.Include(a => a.Manager).FirstOrDefaultAsync(a => a.Id == id);
            return store;
        }

        public async Task<Store> GetStoreByUserId(string userId) {
            var response = await _context.Stores.Include(a=>a.Manager).FirstOrDefaultAsync(a=>a.Manager.Id==userId);
            return response;
        }

        public async Task<List<Store>> GetStoresByName(string name) {
            var stores = await _context.Stores.Where(a => a.Name.Contains(name)).ToListAsync();
            return stores;
        }

        public async Task<bool> IsStoreExist(string name) {
            var result = await _context.Stores.AnyAsync(a => a.Name == name);
            return result;
        }

        public async Task<Store> AddStore(Store newStore) {
            using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    await _context.Stores.AddAsync(newStore);
                    await SaveAsync();
                    var user = _context.Users.First(user => user.Id == newStore.ManagerId);
                    user.ManagedStoreId = newStore.Id;
                    await SaveAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
            return newStore;
        }

        public async Task<UpdateStoreDto> UpdateStore(int id, UpdateStoreDto updatedStore) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            if (store != null) {
                store.Name = updatedStore.Name;
                store.Description = updatedStore.Description;
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return updatedStore;
        }

        public async Task<Store> DeleteStore(int id) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            var response = store;
            if (store != null) {
                _context.Stores.Remove(store);
                await SaveAsync();
            }
            return response;
        }

        public async Task<Store> UpdateVerifyState(int storeId, bool state) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == storeId);
            if (store != null) {
                store.IsVerified = state;
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return store;
        }

        public async Task<Store> UpdateBanState(int storeId, bool state) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == storeId);
            if (store != null) {
                store.IsBanned = state;
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return store;
        }

        public async Task<Store> UpdateOpenState(int storeId, bool state) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == storeId);
            if (store != null) {
                store.IsOpen = state;
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return store;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Store> UpdateStoreImage(int id, string imageUrlResponse) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.ManagedStoreId==id);
            if (store != null && user != null) {
                user.ImgPath = imageUrlResponse;
                store.ImgPath = imageUrlResponse;
                _context.Users.Update(user);
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return store;
        }

        public async Task<Store> UpdateStoreThumbnailImage(int id, string imageUrlResponse) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            if (store != null) {
                store.ThumbnailImgPath = imageUrlResponse;
                _context.Stores.Update(store);
                await SaveAsync();
            }
            return store;
        }

        public async Task<StoreOverviewStatistic> GetStoreOverviewStatistic(int id) {
            var result = new StoreOverviewStatistic();

            var productsCount = await _context.Products.Where(a => a.StoreId == id).CountAsync();

            var ratingCount = await _context.Feedbacks
                .Where(fb => _context.Products
                .Any(p => p.StoreId == id && p.Id == fb.ProductId))
                .CountAsync();

            result.TotalProductCount = productsCount;
            result.TotalProductRatingCount = ratingCount;

            return result;
        }
    }
}
