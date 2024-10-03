using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.BiddingRepository {
    public class BiddingRepository : IBiddingRepository {

        private readonly EXEContext _context;

        public BiddingRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<Bidding>> GetBiddings(int count) {
            var data = await _context.Biddings.Take(count).ToListAsync();
            return data;
        }

        public async Task<Bidding> GetBiddingById(int id) {
            var data = await _context.Biddings
                .FirstOrDefaultAsync(a => a.Id == id);
            return data;
        }

        public async Task<bool> IsValidToAddAuctioneer(int biddingId, int storeId) {
            var bidding = await _context.Biddings
                .Include(a => a.Auctioneers)
                .FirstOrDefaultAsync(a => a.Id == biddingId);
            if (bidding != null) {
                foreach (var item in bidding.Auctioneers) {
                    if (item.StoreId == storeId) {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<List<Auctioneer>> GetAuctioneersByBiddingId(int id) {
            var data = await _context.Auctioneers
                .Include(a => a.Store)
                .Include(a => a.ListImages)
                .Where(a => a.BiddingId == id).ToListAsync();
            return data;
        }

        public async Task<List<string>> GetCategoriesNameByBiddingId(int id) {
            var data = await _context.BiddingCategories
                .Where(a => a.BiddingId == id)
                .Include(a => a.Category)
                .Select(a => a.Category.Name)
                .ToListAsync();
            return data;
        }

        public async Task<List<string>> GetMaterialsNameByBiddingId(int id) {
            var data = await _context.BiddingMaterials
                .Where(a => a.BiddingId == id)
                .Include(a => a.Material)
                .Select(a => a.Material.Name)
                .ToListAsync();
            return data;
        }

        public async Task<Bidding> AddBidding(Bidding bidding, List<int> categoryIds, List<int> materialIds) {
            using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    bidding.IsDone = false;
                    bidding.IsPaid = false;
                    bidding.CreatedDate = DateTime.Now;
                    await _context.Biddings.AddAsync(bidding);
                    await SaveAsync();

                    var listBiddingCategories = new List<BiddingCategory>();

                    foreach (var id in categoryIds) {
                        var biddingCategory = new BiddingCategory();
                        biddingCategory.BiddingId = bidding.Id;
                        biddingCategory.CategoryId = id;
                        listBiddingCategories.Add(biddingCategory);
                    }

                    var listBiddingMaterials = new List<BiddingMaterial>();
                    foreach (var id in materialIds) {
                        var biddingMaterial = new BiddingMaterial();
                        biddingMaterial.BiddingId = bidding.Id;
                        biddingMaterial.MaterialId = id;
                        listBiddingMaterials.Add(biddingMaterial);
                    }
                    await _context.BiddingCategories.AddRangeAsync(listBiddingCategories);
                    await _context.BiddingMaterials.AddRangeAsync(listBiddingMaterials);
                    await SaveAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
            return bidding;
        }

        public async Task<Auctioneer> AddAuctioneer(Auctioneer auctioneer, List<AuctioneerImage> images) {
            try {
                auctioneer.IsChosen = false;
                auctioneer.ListImages = images;
                await _context.Auctioneers.AddAsync(auctioneer);
                await SaveAsync();
                return auctioneer;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
