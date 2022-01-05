namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
    }
}

