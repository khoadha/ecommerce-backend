using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.BiddingRepository {
    public interface IBiddingRepository {
        Task<Bidding> AddBidding(Bidding bidding, List<int> categoryIds, List<int> materialIds);
        Task<List<Bidding>> GetBiddings(int count);
        Task<Bidding> GetBiddingById(int id);
        Task<bool> IsValidToAddAuctioneer(int biddingId, int storeId);
        Task<List<Auctioneer>> GetAuctioneersByBiddingId(int id);
        Task<List<string>> GetMaterialsNameByBiddingId(int id);
        Task<Auctioneer> AddAuctioneer(Auctioneer auctioneer, List<AuctioneerImage> images);
        Task<List<string>> GetCategoriesNameByBiddingId(int id);
        Task<bool> SaveAsync();
    }
}
