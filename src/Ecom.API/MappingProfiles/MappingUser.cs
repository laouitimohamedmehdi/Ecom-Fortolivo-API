using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Orders;

namespace Ecom.API.MappingProfiles
{
    public class MappingUser:Profile
    {
        public MappingUser()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
