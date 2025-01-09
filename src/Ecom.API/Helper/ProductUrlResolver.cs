using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;

namespace Ecom.API.Helper
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductPicture))
            {
                return _config["ApiURL"] + source.ProductPicture;
            }
            return null;
        }
    }
}
