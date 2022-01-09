namespace FreakyFashionServices.OrderProcessor.Models.DTO
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string Customer { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }

        public class OrderLine
        {
            public int Id { get; set; }
            public string ProductId { get; set; }
            public int Quantity { get; set; }
           // public OrderDto Order { get; set; }
        }
    }
}
