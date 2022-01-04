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
        public async Task<IActionResult> CreateOrder(string identifier)
        {
            var basket = GetBasket(identifier);

            if(basket is null) return NotFound();

            var order = new Order(
                
                );

            return Created("", null);
        }

        private async Task<BasketDto> GetBasket(string identifier)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{BasketServiceConnection}{identifier}");

            if(!response.IsSuccessStatusCode) return null;

            var serializedBasket = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<BasketDto>(serializedBasket);
        }
    }
}
