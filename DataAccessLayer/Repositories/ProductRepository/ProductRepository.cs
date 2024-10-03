using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.BusinessModels;

namespace DataAccessLayer.Repositories {
    public class ProductRepository : IProductRepository {

        private readonly EXEContext _context;

        public ProductRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<Product>> SearchProduct(string query) {
            var listProducts = await _context.Products
                .OrderByDescending(a => a.Id)
                .Include(a => a.Store)
                .Where(a => a.Store.IsBanned == false)
                .Where(a => a.Name.Contains(query))
                .ToListAsync();
            return listProducts;
        }

        public async Task<List<Product>> GetProducts() {
            var listProducts = await _context.Products
                .OrderByDescending(a => a.Id)
                .ToListAsync();
            return listProducts;
        }

        public async Task<List<Product>> GetProductsForUser(int count) {
            var listProducts = await _context.Products
                .OrderByDescending(a => a.Id)
                .Include(a => a.Store)
                .Where(a => a.Store.IsBanned == false)
                .Take(count).OrderByDescending(a => a.Id)
                .ToListAsync();
            return listProducts;
        }

        public async Task<List<Product>> GetProductsForUserBySales(int count) {
            var listProducts = await _context.Products
                .OrderByDescending(a => a.Id)
                .Include(a => a.Store)
                .Where(a => a.Store.IsBanned == false)
                .OrderByDescending(a => a.NumberOfSales)
                .Take(count)
                .ToListAsync();
            return listProducts;
        }

        public async Task<Product> GetProductById(int id, bool isViewIncrease = false) {
            var product = await _context.Products
                .Include(a => a.Store)
                .Where(a => a.Store.IsBanned == false)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (isViewIncrease && product is not null) {
                product.ViewCount++;
                _context.Products.Update(product);
                await SaveAsync();
            }
            return product;
        }

        public async Task<ProductRatingData> GetProductRatingData(int productId) {
            var result = new ProductRatingData();
            var averageRating = await _context.Feedbacks
                   .Where(fb => fb.ProductId == productId)
                   .AverageAsync(fb => fb.Rating);

            if (averageRating != null) {
                var fractionalPart = averageRating - Math.Floor((double)averageRating);
                if (fractionalPart > 0.5) {
                    result.AverageRating = (int)Math.Ceiling((double)averageRating);
                } else {
                    result.AverageRating = (int)Math.Floor((double)averageRating);
                }
            } else {
                averageRating = 0;
            }

            result.TotalRating = await _context.Feedbacks
                .Where(fb => fb.ProductId == productId)
                .CountAsync();
            return result;
        }

        public async Task<List<Product>> GetProductsByStoreId(int storeId) {
            var listProducts = await _context.Products.OrderByDescending(a => a.Id).Where(a => a.StoreId == storeId).ToListAsync();
            return listProducts;
        }

        public async Task<List<Product>> GetProductsByStoreIdForUser(int storeId) {
            var listProducts = await _context.Products
                .OrderByDescending(a => a.Id)
                .Include(a => a.Store)
                .Where(a => a.StoreId == storeId && a.Store.IsBanned == false).ToListAsync();
            return listProducts;
        }

        public async Task<List<Product>> GetProductsByName(string name) {
            var products = await _context.Products.Where(a => a.Name.Contains(name)).ToListAsync();
            return products;
        }

        public async Task<Product> AddProduct(Product product, List<ProductImage> images, List<int> categoryIds, List<int> materialIds) {
            using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    product.ViewCount = 0;
                    product.NumberOfSales = 0;
                    product.IsSoldOut = false;
                    product.ListImages = images;
                    product.ImgPath = images.First().Url;
                    await _context.Products.AddAsync(product);
                    await SaveAsync();
                    var store = _context.Stores.FirstOrDefault(a => a.Id == product.StoreId);
                    if (store != null) {
                        store.Products.Add(product);
                    }

                    var listProductCategories = new List<ProductCategory>();

                    foreach (var id in categoryIds) {
                        var productCategory = new ProductCategory();
                        productCategory.ProductId = product.Id;
                        productCategory.CategoryId = id;
                        listProductCategories.Add(productCategory);
                    }

                    var listMaterialCategories = new List<ProductMaterial>();
                    foreach (var id in materialIds) {
                        var productMaterial = new ProductMaterial();
                        productMaterial.ProductId = product.Id;
                        productMaterial.MaterialId = id;
                        listMaterialCategories.Add(productMaterial);
                    }

                    await _context.ProductCategories.AddRangeAsync(listProductCategories);

                    await _context.ProductMaterials.AddRangeAsync(listMaterialCategories);

                    await SaveAsync();

                    await transaction.CommitAsync();  

                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
            return product;
        }

        public async Task<UpdateProductDto> UpdateProduct(int id, UpdateProductDto updatedProduct) {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
            if (product != null) {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                _context.Products.Update(product);
                await SaveAsync();
            }
            return updatedProduct;
        }

        public async Task<Product> UpdateSoldOutState(int productId, bool state) {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == productId);
            if (product != null) {
                product.IsSoldOut = state;
                _context.Products.Update(product);
                await SaveAsync();
            }
            return product;
        }
        public async Task<Product> UpdateProductImage(int id, string imageUrlResponse) {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
            if (product != null) {
                _context.Products.Update(product);
                await SaveAsync();
            }
            return product;
        }

        public async Task<List<string>> GetProductImages(int id) {
            return await _context.ProductImages
                .Where(a => a.ProductId == id)
                .Select(b => b.Url)
                .ToListAsync();
        }

        public async Task<Product> DeleteProduct(int id) {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
            var response = product;
            if (product != null) {
                _context.Products.Remove(product);
                await SaveAsync();
            }
            return response;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Product>> SearchProduct(SearchPayloadDto dto) {

            var query = BuildSearchQuery(dto); ;

            IQueryable<Product> product = query;

            switch (dto.SortOption) {
                case SortEnum.RELEVANCE:
                    break;
                case SortEnum.LOW_TO_HIGH_PRICE:
                    product = product.OrderBy(x => x.Price);
                    break;
                case SortEnum.HIGH_TO_LOW_PRICE:
                    product = product.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }

            var result = await product.Skip(dto.Size * dto.OffSet).Take(dto.Size).ToListAsync();

            return result;
        }

        public async Task<int> GetTotalProductSearch(SearchPayloadDto dto) {

            var total = await BuildSearchQuery(dto).CountAsync();

            return total;
        }

        private IQueryable<Product> BuildSearchQuery(SearchPayloadDto dto) {
            IQueryable<Product> query = _context.Products
                .Include(a => a.Store)
                .Include(a => a.Feedbacks)
                .Include(a => a.ProductCategories)
                .Include(a => a.ProductMaterials)
                .Where(a => a.Store.IsBanned == false);

            /*
             Chạy 2 lệnh này trong local db để enable fulltextsearch
             CREATE FULLTEXT CATALOG ft AS DEFAULT
             CREATE FULLTEXT INDEX ON Products(Name) KEY INDEX PK_Products
             */

            if (!string.IsNullOrEmpty(dto.Search)) {
                query = query.Where(product => EF.Functions.FreeText(product.Name, dto.Search));
            }

            if (dto.FromPrice.HasValue) {
                query = query.Where(product => product.Price >= dto.FromPrice.Value);
            }

            if (dto.ToPrice.HasValue) {
                query = query.Where(product => product.Price <= dto.ToPrice.Value);
            }

            if (dto.Categories != null && dto.Categories.Count() > 0) {
                query = query.Where(product => product.ProductCategories.Any(pc => dto.Categories.Contains(pc.CategoryId)));
            }

            if (dto.Materials != null && dto.Materials.Count() > 0) {
                query = query.Where(product => product.ProductMaterials.Any(pm => dto.Materials.Contains(pm.MaterialId)));
            }

            if (dto.StarOption != null) {
                query = query.Select(product => new {
                    Product = product,
                    AverageRating = product.Feedbacks.Any()
                    ? product.Feedbacks.Average(fb => fb.Rating)
                    : 0
                }).Where(p => p.AverageRating >= dto.StarOption - 1)
                  .Select(p => p.Product);
            }
            return query;
        }


    }
}
