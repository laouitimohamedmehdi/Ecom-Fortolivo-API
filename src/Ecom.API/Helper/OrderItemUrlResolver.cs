using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Orders;

namespace Ecom.API.Helper
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItemOrdered.ProductUrl))
            {
                return _config["ApiURL"] + source.ProductItemOrdered.ProductUrl;
            }
            return null;
        }
    }
}
