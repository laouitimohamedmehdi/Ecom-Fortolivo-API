using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites.Orders;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ecom.API.MappingProfiles
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            CreateMap<ShipAddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductItemId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProductItemName, o => o.MapFrom(s => s.ProductItemOrdered.ProductItemName))
                .ForMember(d => d.ProductUrl, o => o.MapFrom(s => s.ProductItemOrdered.ProductUrl))
                .ForMember(d => d.ProductUrl, o => o.MapFrom<OrderItemUrlResolver>())
                .ReverseMap();
        }
    }
}
