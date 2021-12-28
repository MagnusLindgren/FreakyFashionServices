namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class Order
    {
        public Order(string identifier, string customer)
        {
            Identifier = identifier;
            Customer = customer;
        }

        public int OrderId { get; set; }
        public string Identifier { get; set; }
        public string Customer { get; set; }
    }
}
