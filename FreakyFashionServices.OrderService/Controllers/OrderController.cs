using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Models.Domain;
using FreakyFashionServices.OrderService.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
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

            if (basket.Identifier == null || basket.Items == null)
                return NotFound();

            var newOrder = NewOrder(orderDto.Customer, basket);

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(
               queue: "order",
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);

            var body = Encoding.UTF8
               .GetBytes(JsonSerializer.Serialize(newOrder));

            channel.BasicPublish(

               exchange: "",

               routingKey: "order",
               basicProperties: null,
               body: body);

            return Accepted(new OrderCreatedDto { OrderId = newOrder.OrderId });
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
                OrderId = Guid.NewGuid(),
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
