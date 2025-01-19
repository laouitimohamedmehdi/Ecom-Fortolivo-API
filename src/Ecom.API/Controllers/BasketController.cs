using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;
        public BasketController(IUnitOfWork UOW, IMapper mapper) 
        {
            _uOW = UOW;
            _mapper = mapper;
        }
        [HttpGet("get-basket-item/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetBasketById(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _basket = await _uOW.BasketRepository.GetBasketAsync(id);
                    return Ok(_basket ?? new CustomerBasket(id));
                }
                return NotFound("Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateBasket(CustomerBasketDto customerBasket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result =_mapper.Map<CustomerBasketDto>(customerBasket);
                    var _basket = await _uOW.BasketRepository.UpdateBasketAsync(result);
                    if (_basket is not null)
                    {
                        return Ok(customerBasket);
                    }
                }
                return BadRequest($"Basket Not Found");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("delete-basket-item/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _basket = await _uOW.BasketRepository.GetBasketAsync(id);
                    if (_basket is not null)
                    {
                        var res = await _uOW.BasketRepository.DeleteBasketAsync(id);
                        return Ok(!res ? "Delete Failed" : $"This basket [{id}] is Deleted Successfully");
                    }
                }
                return BadRequest($"Basket Not Found, Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
