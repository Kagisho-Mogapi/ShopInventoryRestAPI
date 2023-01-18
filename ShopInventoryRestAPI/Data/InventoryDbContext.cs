using Microsoft.EntityFrameworkCore;
using ShopInventoryRestAPI.Models;

namespace ShopInventoryRestAPI.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options):base(options)
        {
                
        }

        public DbSet<Item> Items { get; set; }
    }
}
