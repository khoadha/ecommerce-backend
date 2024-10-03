using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace EXE101_API.Services.CategoryMaterialService
{
    public interface ICategoryMaterialService {
        Task<ServiceResponse<List<Category>>> GetCategories();
        Task<ServiceResponse<List<Material>>> GetMaterials();
        Task<ServiceResponse<List<Category>>> GetCategoriesByProductId(int id);
        Task<ServiceResponse<List<Material>>> GetMaterialsByProductId(int id);
        Task<ServiceResponse<Category>> AddCategory(Category category);
        Task<ServiceResponse<Material>> AddMaterial(Material material);
        Task<ServiceResponse<bool>> SaveAsync();

    }
}
