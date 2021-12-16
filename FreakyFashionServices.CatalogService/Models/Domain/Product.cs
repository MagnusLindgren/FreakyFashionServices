namespace FreakyFashionServices.CatalogService.Models.Domain
{
    public class Product
    {
        public Product(string name, string description, int price, string urlSlug)
        {
            Name = name;
            Description = description;
            Price = price;
            UrlSlug = urlSlug;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public  string UrlSlug { get; set; }
    }
}
