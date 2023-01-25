
using Newtonsoft.Json;
using ShopInventory.ConsoleClient;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

HttpClient client = new();
client.BaseAddress = new Uri("https://localhost:7191");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicatin/json"));

HttpResponseMessage response = await client.GetAsync("api/items/all");
response.EnsureSuccessStatusCode();

Item item = null;

if(response.IsSuccessStatusCode)
{
    bool run = true;
    int choice = 0;

    while(run)
    {
        Console.WriteLine();
        Console.WriteLine("-----------Issue interface-----------");
        Console.WriteLine("1. View all items");
        Console.WriteLine("2. Get item details by Id");
        Console.WriteLine("3. Create an item");
        Console.WriteLine("4. Delete an item");
        Console.WriteLine("5. Update an item");
        Console.WriteLine("100. Close item interface");
        choice = int.Parse(Console.ReadLine());


        switch (choice)
        {
            case 1:
                await AllItems();
                break;

            case 2:
                await ItemDetails(); 
                break;

            case 3:
                await CreateItem();
                break;

            case 4:
                await DeleteItem();
                break;

            case 5:
                await UpdateItem();
                break;

            case 100:
                run = false;
                Console.WriteLine("!!! Inventory console client closed !!!");
                break;

            default:
                Console.WriteLine("!!! Wrong choice, choose again please !!!");
                break;
        }
    }


}

else
{
    Console.WriteLine(response.StatusCode.ToString());
}

Console.ReadKey();

async Task AllItems()
{
    response = await client.GetAsync("api/items/all");
    response.EnsureSuccessStatusCode();

    var items = await response.Content.ReadFromJsonAsync<IEnumerable<Item>>();

    foreach(Item item in items)
    {
        Console.WriteLine(item.Id+". "+item.Name);
    }
}

async Task ItemDetails()
{
    int id = 0;
    await AllItems();

    Console.Write("Choose item by id number: ");
    id = int.Parse(Console.ReadLine());

    response = await client.GetAsync($"api/items/details/{id}");
    response.EnsureSuccessStatusCode();
    var item = await response.Content.ReadFromJsonAsync<Item>();

    Console.WriteLine("ID: " + item.Id +
                "\nName: " + item.Name +
                "\nQuantity: " + item.Qty +
                "\nSale: " + item.Sale +"%"+
                "\nDepartment: " + item.Department +
                "\nDiscontinued: " + item.Discontinued);
}

async Task CreateItem()
{
    int detail = 0;
    item = new();

    Console.Write("Enter item name: ");
    item.Name = Console.ReadLine();

    Console.Write("Enter item quantity: ");
    item.Qty = int.Parse(Console.ReadLine());

    Console.Write("Enter item sale percentage: ");
    item.Sale = int.Parse(Console.ReadLine());

    Console.Write("Enter item department(1 = Beauty, 2 = Books, 3 = Fashion, 4 = Health, 5 = HomeAndKitchen, 6 = Music): ");
    detail = int.Parse(Console.ReadLine());
    item.Department = detail - 1 == 0 ? ProductDepartment.Beauty
                : detail - 1 == 1 ? ProductDepartment.Books
                : detail - 1 == 2 ? ProductDepartment.Fashion
                : detail - 1 == 3 ? ProductDepartment.Health
                : detail - 1 == 4 ? ProductDepartment.HomeAndKitchen
                : ProductDepartment.Music;

    item.Discontinued = false;

    var payload = JsonConvert.SerializeObject(item);
    var content = new StringContent(payload, Encoding.UTF8,"application/json");

    response = await client.PostAsync("api/items/create",content);
    response.EnsureSuccessStatusCode();

    Console.WriteLine("--- Item created ---");
}

async Task DeleteItem()
{
    int id = 0;
    await AllItems();

    Console.WriteLine("Choose item to delete by id: ");
    id = int.Parse(Console.ReadLine());

    response = await client.DeleteAsync($"api/items/delete/{id}");
    response.EnsureSuccessStatusCode();

    Console.WriteLine("--- Item deleted ---");
}

async Task UpdateItem()
{
    await AllItems();
    int id = 0;
    char answer = ' ';
    int detail = 0;
    bool discont = false;

    Console.Write("Choose item to update by id: ");
    id= int.Parse(Console.ReadLine());

    response = await client.GetAsync($"api/items/details/{id}");
    response.EnsureSuccessStatusCode();
    var item = await response.Content.ReadFromJsonAsync<Item>();


    Console.Write("Update item Name?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if(answer == 'y')
    {
        Console.Write("Enter item name: ");
        item.Name = Console.ReadLine();
    }

    Console.Write("Update item quantity?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter item quantity: ");
        item.Qty = int.Parse(Console.ReadLine());
    }

    Console.Write("Update item sale?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter item sale: ");
        item.Sale = int.Parse(Console.ReadLine());
    }

    Console.Write("Update item department?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter item department(1 = Beauty, 2 = Books, 3 = Fashion, 4 = Health, 5 = HomeAndKitchen, 6 = Music): ");
        detail = int.Parse(Console.ReadLine());
        item.Department = detail - 1 == 0 ? ProductDepartment.Beauty
                    : detail - 1 == 1 ? ProductDepartment.Books
                    : detail - 1 == 2 ? ProductDepartment.Fashion
                    : detail - 1 == 3 ? ProductDepartment.Health
                    : detail - 1 == 4 ? ProductDepartment.HomeAndKitchen
                    : ProductDepartment.Music;
    }

    Console.Write("Update item discontinuation status?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Is item discontinued?(y/n): ");
        discont = bool.Parse(Console.ReadLine());

        if(discont)
        {
            item.Discontinued = true;
        }
    }

    var payload = JsonConvert.SerializeObject(item);
    var content = new StringContent(payload, Encoding.UTF8, "application/json");

    response = await client.PutAsync($"api/items/update/{id}", content);
    response.EnsureSuccessStatusCode();

    Console.WriteLine("--- Item updated ---");
}