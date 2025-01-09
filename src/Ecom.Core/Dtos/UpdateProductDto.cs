using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class UpdateProductDto: BaseProduct
    {
        public int? CategoryId { get; set; }
        public string OldImage { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
