using AutoMapper;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_API.Services.CategoryMaterialService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class CategoriesAndMaterialsController : ControllerBase
    {

        private readonly ICategoryMaterialService _service;
        private readonly IMapper _mapper;

        public CategoriesAndMaterialsController(ICategoryMaterialService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var listFromDb = await _service.GetCategories();
            var response = _mapper.Map<List<GetCategoryDto>>(listFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("materials")]
        public async Task<IActionResult> GetMaterials()
        {
            var listFromDb = await _service.GetMaterials();
            var response = _mapper.Map<List<GetMaterialDto>>(listFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("categories/{id}")]
        public async Task<IActionResult> GetCategoriesByProductId([FromRoute] int id)
        {
            var listFromDb = await _service.GetCategoriesByProductId(id);
            var response = _mapper.Map<List<GetCategoryDto>>(listFromDb.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("materials/{id}")]
        public async Task<IActionResult> GetMaterialsByProductId([FromRoute] int id)
        {
            var listFromDb = await _service.GetMaterialsByProductId(id);
            var response = _mapper.Map<List<GetMaterialDto>>(listFromDb.Data);
            return Ok(response);
        }



        [HttpPost]
        [Route("categories")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto dto)
        {

            var category = _mapper.Map<Category>(dto);

            var response = await _service.AddCategory(category);

            var result = _mapper.Map<GetCategoryDto>(response.Data);

            return Ok(result);
        }

        [HttpPost]
        [Route("materials")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> AddMaterial([FromBody] AddMaterialDto dto)
        {

            var material = _mapper.Map<Material>(dto);

            var response = await _service.AddMaterial(material);

            var result = _mapper.Map<GetMaterialDto>(response.Data);

            return Ok(result);
        }
    }
}
