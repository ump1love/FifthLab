using Newtonsoft.Json;

class User
{
    private string filepath = "save.json";
    private List<UserData> users = new List<UserData>();
    private static UserData count = new UserData();
    private int currentUser = count.UserCount;

    public User()
    {
        LoadUser();
    }

    private void LoadUser()
    {
        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            users = JsonConvert.DeserializeObject<List<UserData>>(json);
        }
    }

    private void SaveUser()
    {
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }

    public char UserChoice()
    {
        char choice;

        Console.WriteLine("\n" +
                          "User Manager\n" +
                          "Choose what you want to do\n" +
                          $"Your current user is: User#{currentUser + 1}\n" +
                          "1 - Create User\n" +
                          "2 - Delete User\n" +
                          "3 - User Info\n" +
                          "4 - User Modification\n" +
                          "5 - Change Current User\n" +
                          "6 - Exit\n");
        Console.Write("Choice: ");
        choice = Console.ReadKey().KeyChar;

        return choice;
    }

    public bool UserLogin()
    {
        Console.Write("\nEnter your username: ");
        string username = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        UserData user = users.Find(u => u.Username == username);

        if (user != null)
        {
            string hashedPassword = PasswordManager.HashPassword(password, user.Salt);


            if (PasswordManager.VerifyPassword(password, user.HashedPassword, user.Salt))
            {
                currentUser = users.IndexOf(user);
                Console.WriteLine("Login successful");
                return true;
            }
        }

        Console.WriteLine("Invalid username or password");
        return false;
    }

    public void UserCreation()
    {
        UserData newUser = new UserData();

        Console.WriteLine();
        Console.Write("Enter username: ");
        newUser.Username = Console.ReadLine();
        Console.Write("Enter password: ");
        string plainPassword = Console.ReadLine();

        newUser.Salt = PasswordManager.GenerateSalt();
        newUser.HashedPassword = PasswordManager.HashPassword(plainPassword, newUser.Salt);

        newUser.DateOfCreation = DateTime.Now;
        newUser.DateOfModification = DateTime.Now;
        newUser.UserCount = users.Count+1;

        users.Add(newUser);
        SaveUser();
    }

    public void UserDel()
    {
        char choice;

        if (users.Count > 0)
        {
            Console.WriteLine("\n" +
                              $"Are you sure that you want to delete User#{currentUser + 1} (Y/N)?");
            choice = char.ToUpper(Console.ReadKey().KeyChar);
            if (choice == 'Y')
            {
                users.RemoveAt(currentUser);
                SaveUser();
                Console.WriteLine("\n" +
                                  "User has been deleted");
            }
            else { return; }
        }
        else { AbsenceOfTheUsers(); }
    }

    public void UserInfo()
    {
        if (users.Count > 0)
        {
            Console.WriteLine("\n" +
                              "User Information:\n" +
                              $"Username: {users[currentUser].Username}\n" +
                              $"Date of Creation: {users[currentUser].DateOfCreation}\n" +
                              $"Date of Modification: {users[currentUser].DateOfModification}");
            ShowPurchaseHistory();
        }
        else { AbsenceOfTheUsers(); }
    }

    public void UserModification()
    {
        if (users.Count > 0)
        {
            Console.WriteLine("\n" +
                              $"Your current user is: User#{currentUser + 1}\n" +
                              "Choose what do you want to change:\n" +
                              "1 - Username\n" +
                              "2 - Exit\n");
            char userModificationChoice = Console.ReadKey().KeyChar;

            switch (userModificationChoice)
            {
                case '1':
                    UserModificationChanger("Username", nameof(UserData.Username));
                    break;
                case '2': return;
                default: Console.WriteLine("Invalid User Modification input"); return;
            }
        }
        else { AbsenceOfTheUsers(); }
    }

    public void UserModificationChanger(string text, string property)
    {
        string newUserPart;

        object currentValue = typeof(UserData).GetProperty(property)?.GetValue(users[currentUser]);

        Console.WriteLine($"\nYour current {text}: {currentValue}");
        Console.Write($"Write a new {text}: ");
        newUserPart = Console.ReadLine();

        typeof(UserData).GetProperty(property)?.SetValue(users[currentUser], newUserPart);

        users[currentUser].DateOfModification = DateTime.Now;
        SaveUser();
    }

    public void UserChanger()
    {
        int userChoice;
        bool correctUserChoice = false;

        if (users.Count > 0)
        {
            do
            {
                Console.WriteLine("\n" +
                                  $"There are currently {users.Count} users\n");

                for (int i = 0; i < users.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {users[i].Username}");
                }

                Console.Write("Choose the user you want to switch to (enter the user number or 0 for exit): ");
                if (int.TryParse(Console.ReadLine(), out userChoice) && userChoice >= 1 && userChoice <= users.Count)
                {
                    currentUser = userChoice - 1;
                    SaveUser();
                    correctUserChoice = true;
                }
                else if (userChoice == 0) { return; }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid user number.");
                }
            } while (!correctUserChoice);
        }
        else { AbsenceOfTheUsers(); }
    }

    public void AddToPurchaseHistory(Order order)
    {
        users[currentUser].PurchaseHistory.Add(order);
        SaveUser();
    }
    public void ShowPurchaseHistory()
    {
        if (users[currentUser].PurchaseHistory.Count > 0)
        {
            Console.WriteLine("\nPurchase History:");
            foreach (var order in users[currentUser].PurchaseHistory)
            {
                Console.WriteLine($"Order ID: {order.OrderId}\n" +
                                  $"Product Name: {order.Product.Name}\n" +
                                  $"Quantity: {order.Quantity}\n" +
                                  $"Price: {order.Subtotal}\n" +
                                  $"Address: {order.Address}\n" +
                                  $"Order Date: {order.OrderDate}\n" +
                                  $"Order Status: {order.Status}\n");
            }
        }
        else
        {
            Console.WriteLine("No purchase history available.");
        }
    }

    public bool HasUsers()
    {
        return users.Count > 0;
    }

    public void AbsenceOfTheUsers()
    {
        Console.WriteLine("\n" +
                          "There are currently no users.\n" +
                          "If you know that there has to be someone, try to restart this program.\n");
    }

    public void UserExiting()
    {
        Console.WriteLine("\n" +
                          "Exiting...");
    }
}