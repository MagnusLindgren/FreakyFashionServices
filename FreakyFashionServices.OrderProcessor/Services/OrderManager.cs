using FreakyFashionServices.OrderProcessor.Data;
using FreakyFashionServices.OrderProcessor.Models.Domain;

namespace FreakyFashionServices.OrderProcessor.Services
{
    public class OrderManager
    {
        public OrderManager(ApplicationContext context)
        {
            Context = context;
        }
        private ApplicationContext Context { get; }

        public void RegisterOrder(Order order)
        {
            Context.Order.Add(order);

            Context.SaveChanges();
        }
    }
}
