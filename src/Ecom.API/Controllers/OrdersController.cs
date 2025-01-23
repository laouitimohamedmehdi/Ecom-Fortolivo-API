using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.Dtos;
using Ecom.Core.Entites.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IUnitOfWork _uOW;
        private IOrderServices _orderService;
        private IMapper _mapper;

        public OrdersController(IUnitOfWork UOW, IOrderServices orderService, IMapper mapper)
        {
            _uOW = UOW;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder(OrderDto orderdto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var address = _mapper.Map<AddressDto, ShipAddress>(orderdto.ShipAddress);
            var order = await _orderService.CreateOrderAsync(email, orderdto.DeliveryMethodId, orderdto.BasketId, address);

            if (order is null) return BadRequest(new BaseCommonResponse(400, "Error while creating order"));
            return Ok(order);
        }

        [HttpGet("Get-Delivery-Methods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            if (deliveryMethods is null) return BadRequest(new BaseCommonResponse(400, "Error in Delivery Methods"));
            return Ok(deliveryMethods);
        }

        [HttpGet("Get-Order-For-User")]
        public async Task<IActionResult> GetOrderForUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrdersForUserAsync(email);
            if (order is null) return NotFound(new BaseCommonResponse(404, "No Order For This User"));
            var result = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(order);
            return Ok(result);
        }

        [HttpGet("Get-Order-By-Id/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order is null) return NotFound(new BaseCommonResponse(404, "No Order For This User"));
            var result = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(result);
        }

    }
}
