using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace EXE_API.Services.ProductService
{
    public interface IProductService {
        Task<ServiceResponse<List<Product>>> GetProducts();
        Task<ServiceResponse<List<Product>>> GetProductsForUser(int count);
        Task<ServiceResponse<Product>> GetProductById(int id, bool isViewIncrease = false);
        Task<ServiceResponse<ProductRatingData>> GetProductRatingData(int productId);
        Task<ServiceResponse<List<Product>>> GetProductsByStoreId(int storeId);
        Task<ServiceResponse<List<Product>>> GetProductsByStoreIdForUser(int storeId);
        Task<ServiceResponse<List<Product>>> GetProductsByName(string name);
        Task<ServiceResponse<Product>> AddProduct(Product product, List<IFormFile> files, List<int> categoryIds, List<int> materialIds);
        Task<ServiceResponse<Product>> UpdateProductImage(int id, string imageUrlResponse);
        Task<ServiceResponse<UpdateProductDto>> UpdateProduct(int id, UpdateProductDto updatedProduct);
        Task<ServiceResponse<Product>> DeleteProduct(int id);
        Task<ServiceResponse<Product>> UpdateSoldOutState(int productId, bool state);
        Task<ServiceResponse<SearchProductResponse>> SearchProduct(SearchPayloadDto dto);
        Task<ServiceResponse<List<string>>> GetProductImages(int id);
        Task<ServiceResponse<bool>> SaveAsync();
        Task<ServiceResponse<List<Product>>> GetProductsForUserBySales(int count);
    }
}
