[Serializable]
class Order
{
    private int orderId;
    private int quantity;
    private decimal subtotal;
    private string address;
    private DateTime orderDate;
    private Delivery status;
    private Product product;

    public Product Product
    {
        get { return product; }
        set { product = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public decimal Subtotal
    {
        get { return subtotal; }
        set { subtotal = value; }
    }

    public int OrderId
    {
        get { return orderId; }
        set { orderId = value; }
    }

    public string Address
    {
        get { return address; }
        set { address = value; }
    }

    public DateTime OrderDate
    {
        get { return orderDate; }
        set { orderDate = value; }
    }

    public Delivery Status
    {
        get { return status; }
        set { status = value; }
    }

    public enum Delivery
    {
        Pending,
        Shipped,
        Delivered,
    }

    public Order()
    {
        OrderId = 0;
        OrderDate = DateTime.Now;
        Status = Delivery.Pending;

    }


}