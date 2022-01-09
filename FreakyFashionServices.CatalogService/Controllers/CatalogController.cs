using FreakyFashionServices.CatalogService.Data;
using FreakyFashionServices.CatalogService.Models.Domain;
using FreakyFashionServices.CatalogService.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.CatalogService.Controllers
{
    [Route("api")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        public CatalogController(CatalogServiceContext context)
        {
            Context = context;
        }

        private CatalogServiceContext Context { get; set; }

        [HttpPost("products")]
        public IActionResult AddProduct(AddProductDto addProductDto)
        {
            var product = new Product(
                addProductDto.Name, 
                addProductDto.Description,
                addProductDto.ImgUrl,
                addProductDto.Price,
                addProductDto.ArticleNumber,
                addProductDto.UrlSlug
                );

            Context.Add(product);

            Context.SaveChanges();

            return Created("", product);
        }

        [HttpGet("products")]
        public IEnumerable<ProductDto> GetAll()
        {
            var productDtos = Context.Product.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name, 
                Description = x.Description,
                ImgUrl = x.ImgUrl,
                Price = x.Price,
                ArticleNumber = x.ArticleNumber,
                UrlSlug = x.UrlSlug
            });

            return productDtos;
        }
    }
}
