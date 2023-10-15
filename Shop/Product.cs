[Serializable]

class Product
{
    private string name;
    private string description;
    private string category;
    private int price;
    private int quantity;
    private int productId;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Description
    { 
        get { return description; }
        set { description = value; }
    }
    public string Category
    {
        get { return category; }
        set { category = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }
    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
    public int ProductId
    {
        get { return productId; }
        set { productId = value; }
    }

}