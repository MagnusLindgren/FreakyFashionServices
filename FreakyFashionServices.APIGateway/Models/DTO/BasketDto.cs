namespace FreakyFashionServices.APIGateway.Models.DTO
{
    public class BasketDto
    {
        public string Identifier { get; set; }

        public ICollection<Item> Items { get; set; }

        public class Item
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
