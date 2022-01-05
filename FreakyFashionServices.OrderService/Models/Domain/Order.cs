namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public ICollection<OrderLine> OrderLine { get; set; }
    }
}
