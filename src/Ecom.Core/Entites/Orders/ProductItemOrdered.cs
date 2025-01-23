namespace Ecom.Core.Entites.Orders
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
            
        }
        public ProductItemOrdered(int productItemId, string productItemName, string productUrl)
        {
            ProductItemId = productItemId;
            ProductItemName = productItemName;
            ProductUrl = productUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; } 
        public string ProductUrl { get; set; }
    }
}