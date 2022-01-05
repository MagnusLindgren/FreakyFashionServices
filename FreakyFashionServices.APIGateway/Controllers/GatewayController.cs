using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.APIGateway.Controllers
{
    [Route("api")]
    [ApiController]
    public class GatewayController : ControllerBase
    {

        [HttpGet("{products}")]
        public async Task<IActionResult> GetProducts()
        {

            return Ok();
        }
    }
}
