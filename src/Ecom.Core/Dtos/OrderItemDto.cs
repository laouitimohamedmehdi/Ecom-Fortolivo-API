﻿using Ecom.Core.Entites.Orders;

namespace Ecom.Core.Dtos
{
    public class OrderItemDto
    {
        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string ProductUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}