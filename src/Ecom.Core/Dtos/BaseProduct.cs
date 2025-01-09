using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.Dtos
{
    public class BaseProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 9999, ErrorMessage = "Price Limited By {0} and {1}")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} Must Be Number")]
        public decimal? Price { get; set; }
    }
}
