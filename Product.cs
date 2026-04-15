using System;
using System.Linq;
using System.Collections.Generic;

namespace CSharpShell;

using System;

public class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}
