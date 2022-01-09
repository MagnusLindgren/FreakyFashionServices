using FreakyFashionServices.OrderProcessor.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionServices.OrderProcessor.Data
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OrderService;Trusted_Connection=True");
        }
        public DbSet<Order> Order { get; set; }
    }
}
