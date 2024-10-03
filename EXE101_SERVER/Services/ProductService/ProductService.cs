using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;

namespace EXE_API.Services.ProductService
{
    public class ProductService : IProductService {

        private readonly IProductRepository _productRepo;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo, IMapper mapper, IBlobService blobService) {
            _productRepo = productRepo;
            _mapper = mapper;
            _blobService = blobService;
        }
        public async Task<ServiceResponse<Product>> GetProductById(int id, bool isViewIncrease = false) {
            var serviceResponse = new ServiceResponse<Product>();
            try {
                var product = await _productRepo.GetProductById(id, isViewIncrease);
                serviceResponse.Data = product;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts() {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try {
                var listProducts = await _productRepo.GetProducts();
                serviceResponse.Data = listProducts;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<SearchProductResponse>> SearchProduct(SearchPayloadDto dto) {
            var serviceResponse = new ServiceResponse<SearchProductResponse>();
            try {
                var listProducts = await _productRepo.SearchProduct(dto);

                var productDto = _mapper.Map<List<Product>, List<GetProductDto>>(listProducts);

                var totalProduct = await _productRepo.GetTotalProductSearch(dto);

                SearchProductResponse response = new();

                response.Data = productDto;

                response.Total = totalProduct;

                serviceResponse.Data = response;

            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<Product>>> GetProductsForUser(int count) {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try {
                var listProducts = await _productRepo.GetProductsForUser(count);
                serviceResponse.Data = listProducts;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsForUserBySales(int count)
        {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try
            {
                var listProducts = await _productRepo.GetProductsForUserBySales(count);
                serviceResponse.Data = listProducts;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByName(string name) {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try {
                var listProducts = await _productRepo.GetProductsByName(name);
                serviceResponse.Data = listProducts;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByStoreId(int storeId) {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try {
                var listProducts = await _productRepo.GetProductsByStoreId(storeId);
                serviceResponse.Data = listProducts;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByStoreIdForUser(int storeId) {
            var serviceResponse = new ServiceResponse<List<Product>>();
            try {
                var listProducts = await _productRepo.GetProductsByStoreIdForUser(storeId);
                serviceResponse.Data = listProducts;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> AddProduct(Product product, List<IFormFile> images, List<int> categoryIds, List<int> materialIds) {
            var serviceResponse = new ServiceResponse<Product>();
            try {
                var fis = new List<ProductImage>();
                foreach (var file in images) {
                    string callbackUrl = await _blobService.UploadFileAsync(file);
                    ProductImage fi = new() {
                        Url = callbackUrl
                    };
                    fis.Add(fi);
                }

                var response = await _productRepo.AddProduct(product, fis, categoryIds, materialIds);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<UpdateProductDto>> UpdateProduct(int id, UpdateProductDto updatedProduct) {
            var serviceResponse = new ServiceResponse<UpdateProductDto>();
            try {
                var response = await _productRepo.UpdateProduct(id, updatedProduct);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<Product>> DeleteProduct(int id) {
            var serviceResponse = new ServiceResponse<Product>();
            try {
                var response = await _productRepo.DeleteProduct(id);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> UpdateSoldOutState(int productId, bool state) {
            var serviceResponse = new ServiceResponse<Product>();
            try {
                var response = await _productRepo.UpdateSoldOutState(productId, state);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SaveAsync() {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _productRepo.SaveAsync();
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> UpdateProductImage(int id, string imageUrlResponse) {
            var serviceResponse = new ServiceResponse<Product>();
            try {
                var response = await _productRepo.UpdateProductImage(id, imageUrlResponse);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<string>>> GetProductImages(int id) {
            var serviceResponse = new ServiceResponse<List<string>>();
            try {
                var response = await _productRepo.GetProductImages(id);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<ProductRatingData>> GetProductRatingData(int productId) {
            var serviceResponse = new ServiceResponse<ProductRatingData>();
            try {
                var response = await _productRepo.GetProductRatingData(productId);
                serviceResponse.Data = response;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
       
