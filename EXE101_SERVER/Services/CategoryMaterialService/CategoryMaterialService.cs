using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.CategoryMaterialRepository;

namespace EXE101_API.Services.CategoryMaterialService
{
    public class CategoryMaterialService : ICategoryMaterialService {

        private readonly ICategoryMaterialRepository _repo;

        public CategoryMaterialService(ICategoryMaterialRepository repo) {
            _repo = repo;
        }

        public async Task<ServiceResponse<Category>> AddCategory(Category category) {
            var serviceResponse = new ServiceResponse<Category>();
            try {
                var addedCate = await _repo.AddCategory(category);
                serviceResponse.Data = addedCate;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Material>> AddMaterial(Material material) {
            var serviceResponse = new ServiceResponse<Material>();
            try {
                var addedMaterial = await _repo.AddMaterial(material);
                serviceResponse.Data = addedMaterial;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories() {
            var serviceResponse = new ServiceResponse<List<Category>>();
            try {
                var list = await _repo.GetCategories();
                serviceResponse.Data = list;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Material>>> GetMaterials() {
            var serviceResponse = new ServiceResponse<List<Material>>();
            try {
                var list = await _repo.GetMaterials();
                serviceResponse.Data = list;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesByProductId(int id) {
            var serviceResponse = new ServiceResponse<List<Category>>();
            try {
                var list = await _repo.GetCategoriesByProductId(id);
                serviceResponse.Data = list;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Material>>> GetMaterialsByProductId(int id) {
            var serviceResponse = new ServiceResponse<List<Material>>();
            try {
                var list = await _repo.GetMaterialsByProductId(id);
                serviceResponse.Data = list;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<bool>> SaveAsync() {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var result = await _repo.SaveAsync();
                serviceResponse.Data = result;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
