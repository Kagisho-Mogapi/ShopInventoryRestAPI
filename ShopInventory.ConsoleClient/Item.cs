using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopInventory.ConsoleClient
{
    internal class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Qty { get; set; } = 0;
        public ProductDepartment Department { get; set; }
        public int Sale { get; set; }
        public bool Discontinued { get; set; }

    }

    public enum ProductDepartment
    {
        Beauty, Books, Fashion, Health, HomeAndKitchen, Music
    }
}
