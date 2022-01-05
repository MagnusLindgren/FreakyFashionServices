namespace FreakyFashionServices.BasketService.Models.DTO
{
    /*
    public class Items
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class BasketDto
    {
        public string Identifier { get; set; }
        public Items Items { get; set; }
    }
    */
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