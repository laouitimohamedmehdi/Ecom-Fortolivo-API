using Ecom.Core.Entites.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uOW;
        private readonly ApplicationDbContext _context;
        public OrderServices(IUnitOfWork UOW, ApplicationDbContext context)
        {
            _uOW = UOW;
            _context = context;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
        {
            var basket = await _uOW.BasketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;

            var items = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _uOW.ProductRepository.GetByIdAsync(item.Id);
                var productItemOrderd = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.ProductPicture);
                var orderItem = new OrderItem(productItemOrderd, item.Price, item.Quantity);
                items.Add(orderItem);
            }

            //add items to Db
            await _context.OrderItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            //get delivery method
            var deliveryMethod = _context.DeliveryMethods.Where(x => x.Id == deliveryMethodId).FirstOrDefault();

            //calcul subtotal
            var subTotal = items.Sum(x => x.Price * x.Quantity);

            //create order
            var order = new Order(buyerEmail, shipAddress, deliveryMethod, items, subTotal);

            //check order is not null
            if (order is null) return null;

            //add order to Db
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //remove basket
            await _uOW.BasketRepository.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _context.DeliveryMethods.ToListAsync();

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _context.Orders
                .Where(x => x.Id == id && x.BuyerEmail == buyerEmail)
                .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrdered)
                .Include(x => x.DeliveryMethod)
                .OrderByDescending(x => x.OrderDate)
                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var order = await _context.Orders
                .Where(x => x.BuyerEmail == buyerEmail)
                .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrdered)
                .Include(x => x.DeliveryMethod)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
            return order;
        }
    }
}
