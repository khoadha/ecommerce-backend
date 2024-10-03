using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_API.Services.BiddingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers {
    [ApiController]
    [Route("api/v1/")]
    public class BiddingController : ControllerBase {

        private readonly IBlobService _blobService;
        private readonly IBiddingService _biddingService;
        private readonly IMapper _mapper;

        public BiddingController(
            IBlobService blobService,
            IBiddingService biddingService,
            IMapper mapper) {
            _biddingService = biddingService;
            _mapper = mapper;
            _blobService = blobService;
        }

        [HttpPost("bidding")]
        [Authorize]
        public async Task<IActionResult> AddBidding([FromForm] AddBiddingRequestDto model) {
            var bidding = _mapper.Map<Bidding>(model);

            if (model.ImageUrl == null) {
                bidding.ImageUrl = await _blobService.UploadFileAsync(model.Image);
            } else {
                bidding.ImageUrl = await _blobService.UploadFileAsync(model.ImageUrl);
            }
            var serviceResponse = await _biddingService.AddBidding(bidding, model.CategoryIds, model.MaterialIds);
            var response = _mapper.Map<GetBiddingDto>(serviceResponse.Data);
            return Ok(response);
        }

        [HttpGet("bidding")]
        public async Task<IActionResult> GetBiddings([FromQuery] int count) {
            var serviceResponse = await _biddingService.GetBiddings(count);
            var response = _mapper.Map<List<GetBiddingDto>>(serviceResponse.Data);
            return Ok(response);
        }

        [HttpGet("bidding/{id}")]
        public async Task<IActionResult> GetBiddingById([FromRoute] int id) {

            var serviceResponse = await _biddingService.GetBiddingById(id);
            return Ok(serviceResponse.Data);
        }

        [HttpPost("bidding/auctioneer")]
        [Authorize]
        public async Task<IActionResult> AddAuctioneer([FromForm] AddAuctioneerDto dto) {

            var isValid = await _biddingService.IsValidToAddAuctioneer(dto.BiddingId, dto.StoreId);

            if(!isValid.Data) {
                return BadRequest("Add auctioneer request is not valid!");
            }

            var auctioneer = _mapper.Map<Auctioneer>(dto);

            var serviceResponse = await _biddingService.AddAuctioneer(auctioneer, dto.Files);

            return Ok(serviceResponse.Data);
        }

        [HttpGet("bidding/valid")]
        [Authorize]
        public async Task<IActionResult> IsValidAddAuctioneer([FromQuery] int storeId, [FromQuery] int biddingId) {
            var serviceResponse = await _biddingService.IsValidToAddAuctioneer(biddingId, storeId);
            return Ok(serviceResponse.Data);
        }

        [HttpGet("bidding/{id}/auctioneer")]
        public async Task<IActionResult> GetAuctioneersByBiddingId([FromRoute] int id) {
            var serviceResponse = await _biddingService.GetAuctioneersByBiddingId(id);

            var response = _mapper.Map<List<GetAuctioneersByBiddingDto>>(serviceResponse.Data);

            return Ok(response);
        }

    }
}
