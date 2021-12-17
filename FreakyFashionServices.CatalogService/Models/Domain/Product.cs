namespace FreakyFashionServices.CatalogService.Models.Domain
{
    public class Product
    {
        public Product(string name, string description, string imgUrl, int price, string articleNumber, string urlSlug)
        {
            Name = name;
            Description = description;
            ImgUrl = imgUrl;
            Price = price;
            ArticleNumber = articleNumber;
            UrlSlug = urlSlug;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Price { get; set; }
        public string ArticleNumber { get; set; }
        public  string UrlSlug { get; set; }
    }
}
