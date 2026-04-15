public class CartItem
{
    public Product Product;
    public int Quantity;

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public double GetSubtotal()
    {
        return Product.Price * Quantity;
    }
}
