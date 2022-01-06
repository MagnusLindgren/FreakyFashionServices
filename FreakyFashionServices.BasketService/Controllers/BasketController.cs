using FreakyFashionServices.BasketService.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

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

        private IDistributedCache Cache { get; }

        [HttpPut("{identifier}")]
        public IActionResult UpdateBasket(BasketDto basketDto)
        {
            var serializedBasket = JsonSerializer.Serialize(basketDto);

            Cache.SetString(basketDto.Identifier, serializedBasket);

            return NoContent();
        }

        [HttpGet("{identifier}")]
        public ActionResult<BasketDto> GetBasket(string identifier)
        {
            var basket = Cache.Get(identifier);

            if (basket == null)
                return NotFound();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var basketDto = JsonSerializer.Deserialize<BasketDto>(basket, options);

            return Ok(basketDto);
        }
    }
}
