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
            var basket = GetBasket(orderDto.Identifier);

            if(basket == null) 
                return NotFound();

            var newOrder = await Context.Order.AddAsync(new Order()
            {
                Customer = orderDto.Customer,
                OrderLine = (ICollection<OrderLine>)basket
            });
                
            Context.SaveChanges();

            return Created("orderId: ", newOrder.Entity.Id);
        }

        private async Task<BasketDto> GetBasket(string identifier)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{BasketServiceConnection}{identifier}");

            if(!response.IsSuccessStatusCode) return null;

            var serializedBasket = await response.Content.ReadAsStringAsync();

            BasketDto basket = JsonSerializer.Deserialize<BasketDto>(serializedBasket);

            return basket;
        }
    }
}
