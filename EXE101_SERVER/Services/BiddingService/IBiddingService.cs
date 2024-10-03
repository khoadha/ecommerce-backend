using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace EXE101_API.Services.BiddingService
{
    public interface IBiddingService {
        Task<ServiceResponse<Bidding>> AddBidding(Bidding bidding ,List<int> categoryIds, List<int> materialIds);
        Task<ServiceResponse<List<Bidding>>> GetBiddings(int size);
        Task<ServiceResponse<GetBiddingDetailDto>> GetBiddingById(int id);
        Task<ServiceResponse<bool>> IsValidToAddAuctioneer(int biddingId, int storeId);
        Task<ServiceResponse<Auctioneer>> AddAuctioneer(Auctioneer auctioneer, List<IFormFile> files);
        Task<ServiceResponse<List<Auctioneer>>> GetAuctioneersByBiddingId(int id);
    }
}
