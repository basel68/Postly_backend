using App1.API.Data;
using App1.API.Models.Domain;
using App1.API.Models.DTOs;
using App1.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;

namespace App1.API.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> createCategory([FromBody] CreateCategoryRequestDto request)
        {


            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.urlHandle,
            };

            await categoryRepository.CreateAsync(category);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,

            };
            return Ok(categoryDto);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] string? query = null, [FromQuery] string? sortBy=null,
            [FromQuery] string? sortDirection = null, [FromQuery]int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var categories = await categoryRepository.GetAllAsync(query,sortBy,sortDirection,pageNumber, pageSize);
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCatgoryById([FromRoute] Guid id)
        {
            var fetchedCategory = await categoryRepository.GetByIdAsync(id);
            if (fetchedCategory == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = fetchedCategory.Id,
                Name = fetchedCategory.Name,
                UrlHandle = fetchedCategory.UrlHandle,
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            var category = new Category
            {
                Id = id,
                Name = updateCategoryRequestDto.Name,
                UrlHandle = updateCategoryRequestDto.urlHandle
            };
            category=await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
        
            return Ok(response);

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category= await categoryRepository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await categoryRepository.getCountAsync();
            return Ok(count);
        }

    }
}
