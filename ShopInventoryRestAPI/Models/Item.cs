namespace ShopInventoryRestAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Qty { get; set; }=0;
        public ProductDepartment Department { get; set; }
        public int Sale { get; set; }
        public bool Discontinued { get; set; }

    }

    public enum ProductDepartment
    {
        Beauty, Books, Fashion, Health, HomeAndKitchen, Music
    }
}
