using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.BiddingRepository;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;

namespace EXE101_API.Services.BiddingService
{
    public class BiddingService : IBiddingService {

        private readonly IBiddingRepository _biddingRepo;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public BiddingService(IBiddingRepository biddingRepo, IMapper mapper, IBlobService blobService)
        {
            _blobService = blobService;
            _biddingRepo = biddingRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<Bidding>> AddBidding(Bidding bidding, List<int> categoryIds, List<int> materialIds) {
            var serviceResponse = new ServiceResponse<Bidding>();
            try {
                var response = await _biddingRepo.AddBidding(bidding, categoryIds, materialIds);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Bidding>>> GetBiddings(int count) {
            var serviceResponse = new ServiceResponse<List<Bidding>>();
            try {
                var response = await _biddingRepo.GetBiddings(count);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBiddingDetailDto>> GetBiddingById(int id) {
            var serviceResponse = new ServiceResponse<GetBiddingDetailDto>();
            try {
                var response = await _biddingRepo.GetBiddingById(id);

                var getBiddingDetailDto = _mapper.Map<GetBiddingDetailDto>(response);
                getBiddingDetailDto.BiddingCategories = await _biddingRepo.GetCategoriesNameByBiddingId(id);
                getBiddingDetailDto.BiddingMaterials = await _biddingRepo.GetMaterialsNameByBiddingId(id);
                serviceResponse.Data = getBiddingDetailDto;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsValidToAddAuctioneer(int biddingId, int storeId) {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _biddingRepo.IsValidToAddAuctioneer(biddingId, storeId);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Auctioneer>>> GetAuctioneersByBiddingId(int id) {
            var serviceResponse = new ServiceResponse<List<Auctioneer>>();
            try {
                var response = await _biddingRepo.GetAuctioneersByBiddingId(id);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Auctioneer>> AddAuctioneer(Auctioneer auctioneer, List<IFormFile> files) {
            var serviceResponse = new ServiceResponse<Auctioneer>();
            try {
                var ais = new List<AuctioneerImage>();
                foreach (var file in files) {
                    string callbackUrl = await _blobService.UploadFileAsync(file);
                    AuctioneerImage ai = new() {
                        Url = callbackUrl
                    };
                    ais.Add(ai);
                }
                var response = await _biddingRepo.AddAuctioneer(auctioneer, ais);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
