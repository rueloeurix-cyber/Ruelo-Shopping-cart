using System;
using System.Linq;
using System.Collections.Generic;

namespace Get_and_Set;

public class CartItem
{
    private Product product;
    private int quantity;

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

    public CartItem(Product product, int quantity)
    {
        this.product = product;
        this.quantity = quantity;
    }

    public double GetSubtotal()
    {
        return Product.Price * Quantity;
    }
}
