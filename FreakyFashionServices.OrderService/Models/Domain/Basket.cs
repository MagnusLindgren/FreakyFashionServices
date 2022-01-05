namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class Basket
    {
        public string Identifier { get; set; }

        public IEnumerable<Item> Items { get; set; }

        public class Item
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
