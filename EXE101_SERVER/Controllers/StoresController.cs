using AutoMapper;
using AutoMapper.Internal;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE_API.Services.StatisticService;
using EXE_API.Services.StoreService;
using EXE101_API.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EXE101_API.Controllers
{

    [ApiController]
    [Route("api/v1/")]
    public class StoresController : ControllerBase
    {

        private readonly IStoreService _storeService;
        private readonly IStatisticService _statisticService;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public StoresController(
            IBlobService blobService,
            IUserContext userContext,
            IMapper mapper,
            IStatisticService statisticService,
            IStoreService storeService)
        {
            _statisticService = statisticService;
            _storeService = storeService;
            _mapper = mapper;
            _blobService = blobService;
        }

        [HttpGet]
        [Route("store")]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetAllStores()
        {
            var listStoreFromDb = await _storeService.GetStores();
            var response = _mapper.Map<List<GetStoreDto>>(listStoreFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("store/admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetAllStoresForAdmin()
        {

            var listStoreFromDb = await _storeService.GetStoresForAdmin();
            var response = _mapper.Map<List<GetStoreDto>>(listStoreFromDb.Data);
            return Ok(response);
        }


        [HttpGet]
        [Route("store/search/{id}")]
        public async Task<ActionResult<ServiceResponse<GetStoreDto>>> GetStoreById([FromRoute] int id)
        {
            var storeFromDb = await _storeService.GetStoreById(id);
            if (storeFromDb.Data == null)
            {
                return NotFound($"Store with id {id} is not existed!");
            }
            var response = _mapper.Map<GetStoreDto>(storeFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("store/overview-statistic/{id}")]
        public async Task<IActionResult> GetStoreOverviewStatistic([FromRoute] int id) {
            var data = await _storeService.GetStoreOverviewStatistic(id);
            return Ok(data.Data);
        }

        [HttpGet]
        [Route("store/search")]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetStoresByName([FromQuery] string name)
        {
            var storeFromDb = await _storeService.GetStoresByName(name);
            if (storeFromDb.Data == null || storeFromDb.Data.Capacity == 0)
            {
                return NotFound($"Store with name {name} is not existed!");
            }
            var response = _mapper.Map<List<GetStoreDto>>(storeFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("store/statistic/{storeId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetStatisticOfStore([FromRoute] int storeId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var response = await _statisticService.GetStatistic(storeId, fromDate, toDate);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("store/revenue-last-month/{storeId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetRevenueFromLastMonth([FromRoute] int storeId) {
            
            var response = await _statisticService.GetRevenueFromLastMonth(storeId);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("store/statistic-order/{storeId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderCountStatisticOfStore([FromRoute] int storeId) {
           
            var response = await _statisticService.GetOrderCountStatistic(storeId);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("store/statistic-top-product/{storeId}")]
        [Authorize]
        public async Task<IActionResult> GetTopProductStatisticOfStore([FromRoute] int storeId) {
            
            var response = await _statisticService.GetTopProductStatistic(storeId);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("store/statistic-order-admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetOrderCountStatistic() {
            var response = await _statisticService.GetOrderCountStatistic();
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("store/revenue-last-month-admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<List<GetStoreDto>>>> GetRevenueFromLastMonth() {
            var response = await _statisticService.GetRevenueFromLastMonth();
            return Ok(response.Data);
        }

        [HttpPut]
        [Route("store/update/{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<UpdateStoreDto>>> UpdateStore([FromRoute] int id, [FromBody] UpdateStoreDto storeDto)
        {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser.ManagedStoreId != id) {
                return Forbid();
            }

            var storeFromDb = await _storeService.GetStoreById(id);
            if (storeFromDb.Data == null)
            {
                return NotFound($"Store with id {id} is not existed!");
            }

            var response = await _storeService.UpdateStore(id, storeDto);
            return Ok(response.Data);
        }

        [HttpPut]
        [Route("store/{id}/image")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetStoreDto>>> UpdateStoreImage([FromRoute] int id, [FromForm] UpdateStoreImageDto storeDto)
        {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser.ManagedStoreId != id) {
                return Forbid();
            }

            var imageFile = storeDto.Image;
            var storeFromDb = await _storeService.GetStoreById(id);
            if (storeFromDb.Data == null)
            {
                return NotFound($"Store with id {id} is not existed!");
            }
            string imageUrl = storeFromDb.Data.ImgPath;
            bool isDeleted = await _blobService.DeleteBlobsByUrlAsync(imageUrl);
            if (!isDeleted)
            {
                return NotFound("Image is not existed in Storage!");
            }
            var imageUrlResponse = await _blobService.UploadFileAsync(imageFile);
            //=======================================================
            var productResponse = await _storeService.UpdateStoreImage(id, imageUrlResponse);
            //var response = _mapper.Map<GetStoreDto>(productResponse.Data);
            return Ok(imageUrlResponse);
        }

        [HttpPut]
        [Route("store/{id}/thumbnailImage")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetStoreDto>>> UpdateStoreThumbnailImage([FromRoute] int id, [FromForm] UpdateStoreThumbnailImageDto storeDto)
        {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser.ManagedStoreId != id) {
                return Forbid();
            }

            var imageFile = storeDto.ThumbnailImage;
            var storeFromDb = await _storeService.GetStoreById(id);
            if (storeFromDb.Data == null)
            {
                return NotFound($"Store with id {id} is not existed!");
            }
            string imageUrl = storeFromDb.Data.ThumbnailImgPath;
            bool isDeleted = await _blobService.DeleteBlobsByUrlAsync(imageUrl);
            if (!isDeleted)
            {
                return NotFound("Image is not existed in Storage!");
            }
            var imageUrlResponse = await _blobService.UploadFileAsync(imageFile);
            //=======================================================
            var productResponse = await _storeService.UpdateStoreThumbnailImage(id, imageUrlResponse);
            var response = _mapper.Map<GetStoreDto>(productResponse.Data);
            return Ok(response);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut]
        [Route("store/admin/verify/{storeId}")]
        public async Task<IActionResult> VerifyStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateVerifyState(storeId, true);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut]
        [Route("store/admin/unverify/{storeId}")]
        public async Task<IActionResult> UnverifyStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateVerifyState(storeId, false);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut]
        [Route("store/admin/ban/{storeId}")]
        public async Task<IActionResult> BanStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateBanState(storeId, true);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut]
        [Route("store/admin/unban/{storeId}")]
        public async Task<IActionResult> UnbanStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateBanState(storeId, false);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }


        [HttpPut]
        [Route("store/open/{storeId}")]
        public async Task<IActionResult> OpenStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateOpenState(storeId, true);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }

        [HttpPut]
        [Route("store/close/{storeId}")]
        public async Task<IActionResult> CloseStore([FromRoute] int storeId)
        {
            var store = await _storeService.UpdateOpenState(storeId, false);
            var response = _mapper.Map<GetStoreDto>(store.Data);
            return Ok(response);
        }


        [HttpDelete]
        [Route("store/delete/{id}")]
        public async Task<ActionResult<ServiceResponse<GetStoreDto>>> DeleteStore([FromRoute] int id)
        {
            var storeFromDb = await _storeService.GetStoreById(id);
            if (storeFromDb.Data == null)
            {
                return NotFound($"Store with id {id} is not existed!");
            }

            string imageUrl = storeFromDb.Data.ImgPath;
            string thumbnailImageUrl = storeFromDb.Data.ThumbnailImgPath;

            bool isDeleted = await _blobService.DeleteBlobsByUrlAsync(imageUrl, thumbnailImageUrl);

            if (!isDeleted)
            {
                return NotFound("Image is not existed in Storage!");
            }

            var deletedItem = await _storeService.DeleteStore(id);
            var response = _mapper.Map<GetStoreDto>(deletedItem.Data);
            return Ok(response);
        }
    }
}
