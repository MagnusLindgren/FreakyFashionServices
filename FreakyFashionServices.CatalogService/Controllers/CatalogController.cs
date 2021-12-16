using FreakyFashionServices.CatalogService.Data;
using FreakyFashionServices.CatalogService.Models.Domain;
using FreakyFashionServices.CatalogService.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        public CatalogController(CatalogServiceContext context)
        {
            Context = context;
        }

        private CatalogServiceContext Context { get; set; }

        [HttpPost("{products}")]
        public IActionResult AddProduct(AddProductDto addProductDto)
        {
            var product = new Product(
                addProductDto.Name, 
                addProductDto.Description, 
                addProductDto.Price, 
                addProductDto.UrlSlug
                );

            Context.Add(product);

            Context.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        public IEnumerable<ProductDto> GetAll()
        {
            var productDtos = Context.Product.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name, 
                Description = x.Description,
                Price = x.Price,
                UrlSlug = x.UrlSlug
            });

            return productDtos;
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string UrlSlug { get; set; }
    }
}
