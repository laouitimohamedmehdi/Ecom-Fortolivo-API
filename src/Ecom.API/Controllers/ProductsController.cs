using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Helper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IUnitOfWork _uOW;
        private IMapper _mapper;

        public ProductsController(IUnitOfWork UOW, IMapper mapper)
        {
            _uOW = UOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get([FromQuery]ProductParams productParams)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var allProducts = await _uOW.ProductRepository.GetAllAsync(x => x.Category);
                    var allProducts = await _uOW.ProductRepository.GetAllAsync(productParams);
                    if (allProducts is not null)
                    {
                        var result = _mapper.Map<IReadOnlyList<ProductDto>>(allProducts.ProductsDto);

                        return Ok(new Pagination<ProductDto>(productParams.PageNumber, productParams.PageSize, allProducts.TotalItems, result));
                    }
                }
                return BadRequest("Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _uOW.ProductRepository.GetByIdAsync(id, x => x.Category);
                    if (product is not null)
                    {
                        return Ok(_mapper.Map<ProductDto>(product));
                    }
                }
                return NotFound(new BaseCommonResponse(404));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-new-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromForm] CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.AddAsync(productDto);
                    return res ?Ok(productDto) : BadRequest(productDto);
                }
                return BadRequest(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("update-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromForm] UpdateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.UpdateAsync(id, productDto);
                    return res ? Ok(productDto) : BadRequest(productDto);
                }
                return NotFound($"Category Not Found, Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("delete-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _uOW.ProductRepository.DeleteAsyncWithPicture(id);
                    return Ok($"This category [{id}] is Deleted Successfully");
                }
                return NotFound($"Category Not Found, Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
