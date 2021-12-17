using FreakyFashionServices.BasketService.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FreakyFashionServices.BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        public BasketController(IDistributedCache cache)
        {
            Cache = cache;
        }

        public IDistributedCache Cache { get; }

        [HttpPut("{identifier}")]
        public IActionResult UpdateBasket(BasketDto basketDto)
        {
            return NoContent();
        }
    }
}
