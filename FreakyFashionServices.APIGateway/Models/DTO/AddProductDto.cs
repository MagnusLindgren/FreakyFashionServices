namespace FreakyFashionServices.APIGateway.Models.DTO
{
    public class AddProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Price { get; set; }
        public string ArticleNumber { get; set; }
    }
}
