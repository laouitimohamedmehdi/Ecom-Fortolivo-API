using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;

namespace Ecom.API.MappingProfiles
{
    public class MappingBasket : Profile
    {
        public MappingBasket()
        {
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }
    }
}
