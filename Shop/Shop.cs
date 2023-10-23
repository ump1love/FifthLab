using Newtonsoft.Json;

class Shop : ISearchable
{
    User user = new User();
    Order order = new Order();
    private List<Product> products = new List<Product>();
    private List<Order> orders = new List<Order>();
    private string filepathproduct = "products.json";
    private string filepathorder = "orders.json";

    public Shop()
    {
        LoadOrders();
        LoadProducts();
    }


    private void LoadProducts()
    {
        if (File.Exists(filepathproduct))
        {
            string json = File.ReadAllText(filepathproduct);
            products = JsonConvert.DeserializeObject<List<Product>>(json);
        }
    }

    private void SaveProduct()
    {
        string json = JsonConvert.SerializeObject(products, Formatting.Indented);
        File.WriteAllText(filepathproduct, json);
    }
    private void LoadOrders()
    {
        if (File.Exists(filepathorder))
        {
            string json = File.ReadAllText(filepathorder);
            orders = JsonConvert.DeserializeObject<List<Order>>(json);
        }
    }

    private void SaveOrders()
    {
        string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
        File.WriteAllText(filepathorder, json);
    }

    public string ShopTerminal()
    {
        string userTerminalChoice;
        Console.WriteLine("\nWelcome to our anti-GUI shop!\n" +
                          "Our shop uses a terminal as a navigation panel, " +
                          "type \"help\" to get all available commands.\n" +
                          "And by the way, this terminal is VERY sensitive to syntax\n");
        userTerminalChoice = Console.ReadLine().ToLower();

        return userTerminalChoice;
    }

    public void ShopHelp()
    {
        Console.WriteLine("\nUser - Opens user manager\n" +
                          "Products - List of all available products\n" +
                          "Search - search system\n" +
                          "Exit - For exit\n");
    }

    public string ShopUserChoice()
    {
        string userCreationChoice;
        Console.WriteLine("Type what you want to do: log in, sign up or exit:");
        userCreationChoice = Console.ReadLine().ToLower();
        return userCreationChoice;
    }

    public bool ShopUser(string choice)
    {
        switch (choice)
        {
            case "log in":
                if (user.HasUsers())
                {
                    bool ifLoginSuccess = user.UserLogin();
                    if (ifLoginSuccess) { return true; }
                    else { return false; }
                }
                else
                {
                    Console.WriteLine("No users available. Please sign up first.");
                    return false;
                }
            case "sign up": user.UserCreation(); return true;
            case "exit": return false;
            default:
                Console.WriteLine("Invalid choice");
                return false;
        }
    }

    public void ShopProducts()
    {
        ShopProductSave("Laptop", "electronics", "Very handy gadget for studying or working", 1000, 100);
        ShopProductSave("Phone", "electronics", "Cute and powerful gadget for everyday use", 400, 100);
        ShopProductSave("Book", "literature", "Very interesting book", 20, 100);
    }

    public void ShopProductSave(string name, string category, string descrypion, int price, int quantity)
    {
        if (!products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Product temp = new Product();
            temp.Name = name;
            temp.Category = category;
            temp.Price = price;
            temp.Quantity = quantity;
            temp.Description = descrypion;
            temp.ProductId = products.Count + 1;

            products.Add(temp);

            SaveProduct();
        }
    }

    public void ShopAllProducts()
    {
        if(products.Count > 0)
        {
            Console.WriteLine($"\nThere are currently {products.Count} items\n");
            foreach (var product in products)
            {
                Console.Write($"{product.ProductId} {product.Name} \n");
            }
            Console.WriteLine("If you want to select an item and see its description, price, etc. \n" +
                              "Just type this item name or its ID you can also type \"exit\" for exit");
            string productChoice;
            productChoice = Console.ReadLine();
            var selectedProduct = products.FirstOrDefault(p => p.Name.Equals(productChoice, StringComparison.OrdinalIgnoreCase) || p.ProductId.ToString() == productChoice);
            if (selectedProduct != null)
            {
                ShopProductDisplay(selectedProduct);
            }
            else if (productChoice == "exit")
            { return; }
            else { Console.WriteLine("Invalid item choice"); ShopAllProducts(); }
        }
        else { Console.WriteLine("There are no products at this moment");}
    }

    public void ShopProductDisplay(Product temp)
    {
        Console.WriteLine($"\nShopping product display: {temp.Name}\n" +
                          $"Description: {temp.Description}\n" +
                          $"Category: {temp.Category}\n" +
                          $"Price: {temp.Price}\n" +
                          $"Quantity: {temp.Quantity} \n");
        Console.WriteLine("You can order items from this menu, just type \"order\" to do so or exit to go back.");
        string productOrder;
        productOrder = Console.ReadLine().ToLower();
        if (productOrder == "exit") { return; }
        else if (productOrder == "order") { ShopProductOrderConfirmation(temp); }
        else { Console.WriteLine("Invalid input. "); return; }
    }

    public void ShopProductOrderConfirmation(Product temp)
    {
        Console.WriteLine($"You want to buy a {temp.Name}, right?(Y/N)");
        char orderConfirmation;
        orderConfirmation = Console.ReadKey().KeyChar;
        switch(orderConfirmation) { case 'y': case 'Y': ShopOrder(temp) ; break; case 'n': case 'N': return; default: Console.WriteLine("\nWrong input"); return; }
    }
    public void ShopOrder(Product temp)
    {
        Order order = new Order();
        order.Product = temp;
        Console.WriteLine("\nWelcome to order menu.\n" +
                          $"Item: {temp.Name}\n" +
                          "Enter how many do you want to order: ");
        
        int orderQuantity;
        try
        {
            orderQuantity = int.Parse(Console.ReadLine());
        }catch{ Console.WriteLine("Something wrong with entering quantity of the item"); return;}
        if(orderQuantity > temp.Quantity) { Console.WriteLine($"Sorry, but we don't have this many of {temp.Name}(s)"); return; }
        else if (orderQuantity <= temp.Quantity) { order.Quantity = orderQuantity; temp.Quantity -= orderQuantity; SaveProduct(); }
        else { Console.WriteLine("Something wrong with entering quantity of the item"); return; }
        Console.WriteLine("Please enter your address: ");
        char addressConfirmation;
        do
        {
            string orderAddress;
            orderAddress = Console.ReadLine();
            Console.WriteLine($"Your address is {orderAddress}?(Y/N)");
            addressConfirmation = Console.ReadKey().KeyChar;
            switch(addressConfirmation) { case 'y': case 'Y': order.Address = orderAddress; break; case 'n': case 'N': return; default: Console.WriteLine("\nWrong input"); return; }
        } while (addressConfirmation != 'y' && addressConfirmation != 'Y');

        order.Subtotal = orderQuantity * temp.Price;
        order.OrderId = orders.Count + 1;

        orders.Add(order);

        user.AddToPurchaseHistory(order);

        SaveOrders();

        Console.WriteLine("\nEverything is done. Thank you for purchasing!\n" +
                          $"Your order ID is: {order.OrderId}. " +
                          "You can see its status from user menu by typing \'3\'");

    }

    public void ShopSearch()
    {
        Console.WriteLine("Enter search criteria:\n" +
                          "1 - Price\n" +
                          "2 - Category");
        int searchOption;

        if (int.TryParse(Console.ReadLine(), out searchOption))
        {
            switch (searchOption)
            {
                case 1: SearchByPrice(); break;
                case 2: SearchByCategory(); break;
                default: Console.WriteLine("Invalid option"); break;
            }
        }
        else{ Console.WriteLine("Invalid input"); }
    }
    private void SearchByPrice()
    {
        Console.Write("Enter maximum price: ");
        if (int.TryParse(Console.ReadLine(), out int maxPrice))
        {
            var results = products.Where(p => p.Price <= maxPrice);
            DisplaySearchResults(results);
        }
        else{ Console.WriteLine("Invalid input"); }
    }
    private void SearchByCategory()
    {
        Console.Write("Enter category: ");
        string category = Console.ReadLine();
        var results = products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        DisplaySearchResults(results);
    }
    private void DisplaySearchResults(IEnumerable<Product> results)
    {
        if (results.Any())
        {
            Console.WriteLine("Search results:");
            foreach (var product in results)
            {
                Console.WriteLine($"Product ID: {product.ProductId}\n" +
                                  $"Name: {product.Name}\n" +
                                  $"Price: {product.Price}\n");
            }
        }
        else{ Console.WriteLine("No matching products found."); }
    }
}