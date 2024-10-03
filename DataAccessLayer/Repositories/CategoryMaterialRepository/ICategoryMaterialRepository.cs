using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.CategoryMaterialRepository {
    public interface ICategoryMaterialRepository {
        Task<List<Category>> GetCategories();
        Task<List<Material>> GetMaterials();
        Task<List<Category>> GetCategoriesByProductId(int id);
        Task<List<Material>> GetMaterialsByProductId(int id);
        Task<Category> AddCategory(Category category);
        Task<Material> AddMaterial(Material material);
        Task<bool> SaveAsync();

    }
}
