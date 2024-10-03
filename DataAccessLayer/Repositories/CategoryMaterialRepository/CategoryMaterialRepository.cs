using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.CategoryMaterialRepository {
    public class CategoryMaterialRepository : ICategoryMaterialRepository {

        private readonly EXEContext _context;

        public CategoryMaterialRepository(EXEContext context) {
            _context = context;
        }

        public async Task<Category> AddCategory(Category category) {
            try {
                await _context.Categories.AddAsync(category);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return category;
        }

        public async Task<Material> AddMaterial(Material material) {
            try {
                await _context.Materials.AddAsync(material);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return material;
        }

        public async Task<List<Category>> GetCategories() {
            List<Category> result = new List<Category>();
            try {
                result = await _context.Categories.ToListAsync();
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<List<Material>> GetMaterials() {
            List<Material> result = new List<Material>();
            try {
                result = await _context.Materials.ToListAsync();
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<List<Category>> GetCategoriesByProductId(int id) {
            List<Category> result = new List<Category>();
            try {
                result = await _context.ProductCategories.Where(a => a.ProductId == id).Select(a => a.Category).ToListAsync();
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<List<Material>> GetMaterialsByProductId(int id) {
            List<Material> result = new List<Material>();
            try {
                result = await _context.ProductMaterials.Where(a => a.ProductId == id).Select(a => a.Material).ToListAsync();
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
