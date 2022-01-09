using FreakyFashionServices.APIGateway.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using CatalogService = FreakyFashionServices.APIGateway.Models.DTO.Catalogservice;

namespace FreakyFashionServices.APIGateway.Controllers
{
    [Route("api")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public GatewayController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // Order
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var orderJson = new StringContent(
                JsonSerializer.Serialize(orderDto),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.PostAsync("http://localhost:5100/api/order", orderJson);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await JsonSerializer.DeserializeAsync<OrderCreatedDto>(contentStream, options);

            return Accepted(response.Id);
        }

        // Basket
        [HttpPut("baskets/{identifier}")]
        public async Task<IActionResult> UpdateBasket(string identifier, BasketDto basketDto)
        {
            var basketJson = new StringContent(
                JsonSerializer.Serialize(basketDto),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.PutAsync("http://localhost:5200/api/basket/{identifier}", basketJson);

            return NoContent();
        }

        [HttpGet("baskets/{identifier}")]
        public async Task<IActionResult> GetBasket(string identifier)
        {
            var basket = await FetchBasket(identifier);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }

        private async Task<BasketDto> FetchBasket(string identifier)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5200/api/basket/{identifier}")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            BasketDto basketDto = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
                return basketDto;

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            basketDto = await JsonSerializer.DeserializeAsync<BasketDto>(contentStream, options);

            return basketDto;
        }

        // Products (Catalog)
        [HttpPost("products")]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            var productJson = new StringContent(
                JsonSerializer.Serialize(addProductDto),
                Encoding.UTF8,
                Application.Json
                );

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.PostAsync("http://localhost:5000/api/Catalog/products", productJson);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var catalogServiceProductDto = await JsonSerializer.DeserializeAsync<CatalogService.ProductDto>(contentStream, options);

            return Created("", catalogServiceProductDto);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var productsDto = await FetchProducts();

            var stockLevel = await FetchStock();

            var result = productsDto.Select(x =>
            {
                var stockInfo = stockLevel.FirstOrDefault(stockLevel => stockLevel.ArticleNumber == x.ArticleNumber);
                x.Stock = stockInfo?.Stock ?? 0;
                return x;
            });

            return Ok(result);
        }
                
        private async Task<IEnumerable<ProductDto>> FetchProducts()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5000/api/Catalog/")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            ProductsDto productDtos = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var catalogServiceProductDto = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDto>>(contentStream, options);
            /*
            productDtos = new ProductsDto
            {
                Products = catalogServiceProductDto.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImgUrl = x.ImgUrl,
                    Price = x.Price,
                    ArticleNumber = x.ArticleNumber,
                    UrlSlug = x.UrlSlug,
                })
            };*/

            return catalogServiceProductDto;
        }

        private async Task<IEnumerable<StockLevelDto>> FetchStock()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5300/api/stock/")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            StockLevelDto stockLevelDto = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                stockLevelDto = new StockLevelDto
                { Stock = 0 };
                return (IEnumerable<StockLevelDto>)stockLevelDto;
            }
                
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var stockServiceStockLevelDto = await JsonSerializer.DeserializeAsync<IEnumerable<StockLevelDto>>(contentStream, options);

            return stockServiceStockLevelDto;
        }
    }
}
