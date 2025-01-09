using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class ReturnProductDto
    {
        public int TotalItems { get; set; }
        public List<ProductDto> ProductsDto { get; set; }
    }
}
