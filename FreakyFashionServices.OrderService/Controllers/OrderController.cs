using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Models.Domain;
using FreakyFashionServices.OrderService.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderController(OrderServiceContext context)
        {
            Context = context;
        }

         private OrderServiceContext Context { get; set; }
        public IActionResult CreateOrder(OrderDto orderDto)
        {
            var order = new Order(
                orderDto.Identifier,
                orderDto.Customer
                );

            Context.Add(order);

            Context.SaveChanges();

            return Created("", null);
        }
    }
}
