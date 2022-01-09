namespace FreakyFashionServices.OrderProcessor.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }

        public  class OrderLine
        {
            public int Id { get; set; }
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
