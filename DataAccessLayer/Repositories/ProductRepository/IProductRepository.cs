using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace DataAccessLayer.Repositories {
    public interface IProductRepository {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetProductsForUser(int count);
        Task<List<Product>> SearchProduct(string query);
        Task<Product> GetProductById(int id, bool isViewIncrease = false);
        Task<ProductRatingData> GetProductRatingData(int productId);
        Task<List<Product>> GetProductsByStoreId(int storeId);
        Task<List<Product>> GetProductsByStoreIdForUser(int storeId);
        Task<List<Product>> GetProductsByName(string name);
        Task<Product> AddProduct(Product product, List<ProductImage> files, List<int> categoryIds, List<int> materialIds);
        Task<Product> UpdateProductImage(int id, string imageUrlResponse);
        Task<UpdateProductDto> UpdateProduct(int id, UpdateProductDto updatedProduct);
        Task<Product> UpdateSoldOutState(int productId, bool state);
        Task<Product> DeleteProduct(int id);
        Task<List<Product>> SearchProduct(SearchPayloadDto dto);
        Task<int> GetTotalProductSearch(SearchPayloadDto dto);
        Task<List<string>> GetProductImages(int id);
        Task<bool> SaveAsync();

        Task<List<Product>> GetProductsForUserBySales(int count);
    }
}
