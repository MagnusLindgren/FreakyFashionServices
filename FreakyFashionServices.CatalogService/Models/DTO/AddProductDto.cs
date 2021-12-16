namespace FreakyFashionServices.CatalogService.Models.DTO
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string UrlSlug { get { return Name.Trim().Replace("-", "").Replace(" ", "-").ToLower(); } }
    }
}
