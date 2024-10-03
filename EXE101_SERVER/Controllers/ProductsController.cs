using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE_API.Services.ProductService;
using EXE101_API.Context;
using EXE101_API.Services.CacheService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly ICacheService _cacheService;
        private readonly IUserContext _userContext;

        public ProductsController(
            IUserContext userContext,
            IBlobService blobService,
            IMapper mapper,
            IProductService productService,
            ICacheService cacheService)
        {
            _userContext = userContext;
            _productService = productService;
            _mapper = mapper;
            _blobService = blobService;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("product")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProducts()
        {
            var listProductFromDb = await _productService.GetProducts();
            var response = _mapper.Map<List<GetProductDto>>(listProductFromDb.Data);
            return Ok(response);
        }

        [HttpPost("product-search")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> SearchProduct([FromBody] SearchPayloadDto dto)
        {

            var serviceResponse = await _productService.SearchProduct(dto);

            var response = serviceResponse.Data;

            return Ok(response);
        }


        [HttpGet]
        [Route("product/user")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProductsForUser(int count)
        {
            var listProductFromDb = await _productService.GetProductsForUser(count);
            var response = _mapper.Map<List<GetProductDto>>(listProductFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("product/userBySale")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProductsForUserBySale(int count)
        {
            var listProductFromDb = await _productService.GetProductsForUserBySales(count);
            var response = _mapper.Map<List<GetProductDto>>(listProductFromDb.Data);
            return Ok(response);
        }


        [HttpGet]
        [Route("product/search/{id}")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> GetProductById([FromRoute] int id)
        {
            var productFromDb = await _productService.GetProductById(id, true);
            if (productFromDb.Data == null)
            {
                return NotFound($"Product with id {id} is not existed!");
            }
            var response = _mapper.Map<GetProductDetailPageDto>(productFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("product/rating/{id}")]
        public async Task<IActionResult> GetProductRatingData([FromRoute] int id) {
            var data = await _productService.GetProductRatingData(id);
            return Ok(data.Data);
        }

        [HttpGet]
        [Route("product/searchByStore/{storeId}")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProductsByStoreId([FromRoute] int storeId)
        {
            var productsFromDb = await _productService.GetProductsByStoreId(storeId);
            if (productsFromDb.Data == null)
            {
                return NotFound($"Store with id {storeId} is not existed!");
            }
            var response = _mapper.Map<List<GetProductDto>>(productsFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("product/user/searchByStore/{storeId}")]
        public async Task<ActionResult<ServiceResponse<List<GetProductDto>>>> GetProductsByStoreIdForUser([FromRoute] int storeId)
        {
            var productsFromDb = await _productService.GetProductsByStoreIdForUser(storeId);
            if (productsFromDb.Data == null)
            {
                return NotFound($"Store with id {storeId} is not existed!");
            }
            var response = _mapper.Map<List<GetProductDto>>(productsFromDb.Data);
            return Ok(response);
        }

        [HttpPost("product/add")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<AddProductDto>>> AddProduct([FromForm] AddProductDto productDto)
        {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if(productDto.StoreId != currentUser.ManagedStoreId) {
                return Forbid();
            }

            var product = _mapper.Map<Product>(productDto);

            var serviceResponse = await _productService.AddProduct(product, productDto.Files, productDto.CategoryIds, productDto.MaterialIds);

            var response = _mapper.Map<GetProductDto>(serviceResponse.Data);

            return Ok(response);
        }

        [HttpPut]
        [Route("product/update/{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<UpdateProductDto>>> UpdateStore([FromRoute] int id, [FromBody] UpdateProductDto productDto)
        {
            var productFromDb = await _productService.GetProductById(id);
            if (productFromDb.Data == null)
            {
                return NotFound($"Product with id {id} is not existed!");
            }
            var response = await _productService.UpdateProduct(id, productDto);
            return Ok(response.Data);
        }

        [HttpPut]
        [Route("product/soldOut/{productId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> UpdateSoldOutState([FromRoute] int productId)
        {
            var product = await _productService.UpdateSoldOutState(productId, true);
            var response = _mapper.Map<GetProductDto>(product.Data);
            return Ok(response);
        }

        [HttpPut]
        [Route("product/inStock/{productId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> UpdateInStockState([FromRoute] int productId)
        {
            var product = await _productService.UpdateSoldOutState(productId, false);
            var response = _mapper.Map<GetProductDto>(product.Data);
            return Ok(response);
        }

        [HttpDelete]
        [Route("product/delete/{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> DeleteProduct([FromRoute] int id)
        {
            var productFromDb = await _productService.GetProductById(id);
            if (productFromDb.Data == null)
            {
                return NotFound($"Product with id {id} is not existed!");
            }
            string imageUrl = productFromDb.Data.ImgPath;
            bool isDeleted = await _blobService.DeleteBlobsByUrlAsync(imageUrl);
            if (!isDeleted)
            {
                return NotFound("Image is not existed in Storage!");
            }

            var deletedItem = await _productService.DeleteProduct(id);
            var response = _mapper.Map<GetProductDto>(deletedItem.Data);
            return Ok(response);
        }

        [HttpPut]
        [Route("product/{id}/image")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<UpdateProductImageDto>>> UpdateProductImage([FromRoute] int id, [FromForm] UpdateProductImageDto productDto)
        {
            var imageFile = productDto.Image;
            var productFromDb = await _productService.GetProductById(id);
            if (productFromDb.Data == null)
            {
                return NotFound($"Product with id {id} is not existed!");
            }
            string imageUrl = productFromDb.Data.ImgPath;
            bool isDeleted = await _blobService.DeleteBlobsByUrlAsync(imageUrl);
            if (!isDeleted)
            {
                return NotFound("Image is not existed in Storage!");
            }
            var imageUrlResponse = await _blobService.UploadFileAsync(imageFile);
            var productResponse = await _productService.UpdateProductImage(id, imageUrlResponse);
            var response = _mapper.Map<GetProductImageDto>(productResponse.Data);
            return Ok(response);
        }

        [HttpGet("product/images/{id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] int id) {
            var response = await _productService.GetProductImages(id);
            var datas = new { images = response.Data };
            return Ok(datas);
        }

    }
}

