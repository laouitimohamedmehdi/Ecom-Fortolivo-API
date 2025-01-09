using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private IMapper _mapper;

        public CategoriesController(IUnitOfWork UOW, IMapper mapper)
        {
            _uOW = UOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async  Task<ActionResult> Get()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var allCategories = await _uOW.CategoryRepository.GetAllAsync();
                    if (allCategories is not null)
                    {
                        //var res = allCategories.Select(x => new ListingCategoryDto
                        //{
                        //    Id = x.Id,
                        //    Name = x.Name,
                        //    Description = x.Description
                        //}).ToList();

                        var res = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ListingCategoryDto>>(allCategories);
                        return Ok(res);
                    }
                }
                return NotFound("Not Found");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-category-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categorie = await _uOW.CategoryRepository.GetAsync(id);
                    if (categorie is not null)
                    {
                        //var res = new ListingCategoryDto()
                        //{
                        //    Id = categories.Id,
                        //    Name = categories.Name,
                        //    Description = categories.Description
                        //};

                        return Ok(_mapper.Map<Category, ListingCategoryDto>(categorie));
                    }
                }
                return NotFound($"Not Found this id [{id}]");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-new-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var newCategory = new Category()
                    //{
                    //    Name = categoryDto.Name,
                    //    Description = categoryDto.Description
                    //};
                    var newCategory = _mapper.Map<Category>(categoryDto);
                    await _uOW.CategoryRepository.AddAsync(newCategory);
                    return Ok(categoryDto);
                }
                return BadRequest(categoryDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("update-category-by-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(UpdateCategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = await _uOW.CategoryRepository.GetAsync(categoryDto.Id);
                    if (existingCategory is not null)
                    {
                        //existingCategory.Name = categoryDto.Name;
                        //existingCategory.Description = categoryDto.Description;
                        _mapper.Map(categoryDto, existingCategory);
                        await _uOW.CategoryRepository.UpdateAsync(categoryDto.Id, existingCategory);
                        return Ok(categoryDto);
                    }
                }
                return BadRequest($"Category Not Found, Id [{categoryDto.Id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("delete-category-by-id/{id}\"")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = await _uOW.CategoryRepository.GetAsync(id);
                    if (existingCategory is not null)
                    {
                        await _uOW.CategoryRepository.DeleteAsync(id);
                        return Ok($"This category [{existingCategory.Name}] is Deleted Successfully");
                    }
                }
                return BadRequest($"Category Not Found, Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
