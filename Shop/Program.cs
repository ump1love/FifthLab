class Program
{
    public static void Main()
    {
        Shop shop = new Shop();
        User user = new User();


        string choice;
        string userLoginChoice;
        char userManagerChoice;

        do
        {
            choice = shop.ShopTerminal();
            switch (choice)
            {
                case "help": shop.ShopHelp(); break;
                case "user": userLoginChoice = shop.ShopUserChoice(); 
                    bool ifLoginSuccess = shop.ShopUser(userLoginChoice);
                    if (ifLoginSuccess)
                    {
                        do
                        {
                            userManagerChoice = user.UserChoice();
                            switch (userManagerChoice)
                            {
                                case '1': user.UserCreation(); break;
                                case '2': user.UserDel(); break;
                                case '3': user.UserInfo(); break;
                                case '4': user.UserModification(); break;
                                case '5': user.UserChanger(); break;
                                case '6': user.UserExiting(); break;
                                default: Console.WriteLine("There is an error in userManagerChoice\n"); break;
                            }
                        } while (userManagerChoice != '6');
                    }
                    break;
                case "products": shop.ShopProducts(); shop.ShopAllProducts(); break;
                case "search": shop.ShopSearch(); break;
                case "exit": Console.WriteLine("Exiting... "); break;
                default: Console.WriteLine("Invalid command entered"); break;
            }
        } while (choice != "exit");
    }
}