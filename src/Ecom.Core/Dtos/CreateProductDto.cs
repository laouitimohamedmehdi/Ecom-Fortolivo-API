using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;

namespace Ecom.Core.Dtos
{
    public class CreateProductDto: BaseProduct
    {
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
