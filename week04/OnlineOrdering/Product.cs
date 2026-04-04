public class Product
{
    private string _name;
    private string _productId;
    private double _pricePerUnit;
    private int    _quantity;

    public Product(string name, string productId, double pricePerUnit, int quantity)
    {
        _name         = name;
        _productId    = productId;
        _pricePerUnit = pricePerUnit;
        _quantity     = quantity;
    }

    public string GetName()      => _name;
    public string GetProductId() => _productId;

    public double GetTotalCost()
    {
        return _pricePerUnit * _quantity;
    }
}
