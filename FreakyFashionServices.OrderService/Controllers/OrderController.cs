using AutoMapper;
using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Models.Domain;
using FreakyFashionServices.OrderService.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FreakyFashionServices.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private OrderServiceContext Context { get; set; }

        private readonly string BasketServiceConnection;

        public OrderController(OrderServiceContext context, IConfiguration configuration)
        {
            Context = context;
            BasketServiceConnection = configuration.GetConnectionString("BasketServiceConnection");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var basket = await GetBasket(orderDto.Identifier);

            if(basket.Identifier == null || basket.Items == null) 
                return NotFound();

            var newOrder = NewOrder(orderDto.Customer, basket);

            Context.Add(newOrder);
                
            Context.SaveChanges();

            return Created("", newOrder.Id);
        }

        private async Task<BasketDto> GetBasket(string identifier)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{BasketServiceConnection}{identifier}");

            if(!response.IsSuccessStatusCode) 
                return null;

            var serializedBasket = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<BasketDto>(serializedBasket, options);
        }

        private Order NewOrder(string customer, BasketDto basketDto)
        {
            return new Order
            {
                Customer = customer,
                OrderLines = basketDto.Items.Select(x => new Order.OrderLine
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
        }
    }
}
