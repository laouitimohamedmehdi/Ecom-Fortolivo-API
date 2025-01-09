using Ecom.Core.Entites;

namespace Ecom.Core.Dtos
{
    public class ProductDto: BaseProduct
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }
    }
}
