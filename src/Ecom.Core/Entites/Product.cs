using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string ProductPicture { get; set; }

        //Navigation property

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } 
    }
}
