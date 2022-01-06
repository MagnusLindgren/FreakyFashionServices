using FreakyFashionServices.APIGateway.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
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

        [HttpPost("products")]
        public async Task<IActionResult> AddProduct()
        {
            return Ok();
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var productsDto = await FetchProducts();

            return Ok(productsDto);
        }

        private async Task<ProductsDto> FetchProducts()
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
                return productDtos;

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var catalogServiceProductDto = await JsonSerializer.DeserializeAsync<List<CatalogService.ProductDto>>(contentStream, options);

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
                    Stock = FetchStock(x.ArticleNumber).Result.StockLevel
                })
            };

            return productDtos;
        }

        private async Task<StockLevelDto> FetchStock(string articleNumber)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5300/api/stock/{articleNumber}")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            StockLevelDto stockLevelDto = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                stockLevelDto = new StockLevelDto
                { ArticleNumber = articleNumber, StockLevel = 0 };
                return stockLevelDto;
            }
                

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var stockServiceStockLevelDto = await JsonSerializer.DeserializeAsync<List<CatalogService.ProductDto>>(contentStream, options);

            return stockLevelDto;
        }
    }
}
