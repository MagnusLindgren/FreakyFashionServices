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

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var productDto = await FetchProducts();

            return Ok(productDto);
        }

        private async Task<ProductDto> FetchProducts()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5000/api/Catalog/")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            ProductDto productDto = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
                return productDto;

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var catalogServiceProductDto = await JsonSerializer.DeserializeAsync<CatalogService.ProductDto>(contentStream, options);

            productDto = new ProductDto
            {
                Name = catalogServiceProductDto.Name,
                Description = catalogServiceProductDto.Description,
                ImgUrl = catalogServiceProductDto.ImgUrl,
                Price = catalogServiceProductDto.Price,
                ArticleNumber = catalogServiceProductDto.ArticleNumber,
                UrlSlug = catalogServiceProductDto.UrlSlug
            };

            return productDto;
        }
    }
}
