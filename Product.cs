using System;
using System.Linq;
using System.Collections.Generic;

namespace Get_and_Set;

public class Product
{
    private int id;
    private string name;
    private double price;
    private int remainingStock;
    private string category;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public double Price
    {
        get { return price; }
        set { price = value; }
    }

    public int RemainingStock
    {
        get { return remainingStock; }
        set { remainingStock = value; }
    }

    public string Category
    {
        get { return category; }
        set { category = value; }
    }

    public Product(int id, string name, double price, int stock, string category)
    {
        this.id = id;
        this.name = name;
        this.price = price;
        this.remainingStock = stock;
        this.category = category;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock}) [{Category}]");
    }

    public bool HasEnoughStock(int qty)
    {
        return qty <= RemainingStock;
    }

    public void DeductStock(int qty)
    {
        RemainingStock -= qty;
    }
}
