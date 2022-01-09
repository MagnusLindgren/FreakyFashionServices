using System.ComponentModel.DataAnnotations.Schema;

namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }
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
