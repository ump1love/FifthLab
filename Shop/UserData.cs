[Serializable]
class UserData
{
    private int userCount;
    private string username;
    private string salt;
    private string hashedPassword;
    private List<string> purchaseHistory;
    private DateTime dateOfCreation;
    private DateTime dateOfModification;

    public UserData()
    {
        userCount = 0;
        purchaseHistory = new List<string>();
    }

    public string Username
    {
        get { return username; }
        set { username = value; }
    }

    public string Salt
    {
        get { return salt; }
        set { salt = value; }
    }

    public string HashedPassword
    {
        get { return hashedPassword; }
        set { hashedPassword = value; }
    }

    public List<string> PurchaseHistory
    {
        get { return purchaseHistory; }
        set { purchaseHistory = value; }
    }

    public int UserCount
    {
        get { return userCount; }
        set { userCount = value; }
    }

    public DateTime DateOfCreation
    {
        get { return dateOfCreation; }
        set { dateOfCreation = value; }
    }

    public DateTime DateOfModification
    {
        get { return dateOfModification; }
        set { dateOfModification = value; }
    }
}