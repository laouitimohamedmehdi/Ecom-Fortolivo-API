using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly IMapper _mapper;
        public BasketRepository(IConnectionMultiplexer redis, IMapper mapper)
        {
            _database = redis.GetDatabase();
            _mapper = mapper;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var check = _database.KeyExistsAsync(basketId);
            return !check.Result ? false : await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasketDto customerBasket)
        {
            var _basket = await _database.StringSetAsync(customerBasket.Id, JsonSerializer.Serialize(customerBasket),TimeSpan.FromDays(30));
            return !_basket ? null : await GetBasketAsync(customerBasket.Id);
        }
    }
}
