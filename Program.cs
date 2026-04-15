using System;

class Program
{
    static void Main()
    {
        Product[] products = new Product[]
        {
            new Product(1, "Sofa", 30000, 5),
            new Product(2, "Cushion", 500, 10),
            new Product(3, "Microwave", 1500, 7),
            new Product(4, "Mini fridge", 2000, 3)
        };

        CartItem[] cart = new CartItem[10];
        int cartCount = 0;

        bool continueShopping = true;

        while (continueShopping)
        {
            bool allOutOfStock = true;

            foreach (var p in products)
            {
                if (p.RemainingStock > 0)
                {
                    allOutOfStock = false;
                    break;
                }
            }

            if (allOutOfStock)
            {
                Console.WriteLine("\nAll items are out of stock.");
                break;
            }
            Console.WriteLine("\n--- STORE MENU ---");
            foreach (var p in products)
                p.DisplayProduct();

            Console.Write("Enter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int productChoice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            if (productChoice < 1 || productChoice > products.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }

            Product selectedProduct = products[productChoice - 1];

            if (selectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("Out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selectedProduct.HasEnoughStock(quantity))
            {
                Console.WriteLine("Not enough stock.");
                continue;
            }

            bool found = false;
            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Product.Id == selectedProduct.Id)
                {
                    cart[i].Quantity += quantity;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                cart[cartCount++] = new CartItem(selectedProduct, quantity);
            }

            selectedProduct.DeductStock(quantity);
            Console.WriteLine("Added to cart.");

            Console.Write("Add another item? (Y/N): ");
            continueShopping = Console.ReadLine().ToUpper() == "Y";
        }

        double total = 0;
        Console.WriteLine("\n--- RECEIPT ---");

        for (int i = 0; i < cartCount; i++)
        {
            double subtotal = cart[i].GetSubtotal();
            total += subtotal;
            Console.WriteLine($"{cart[i].Product.Name} x{cart[i].Quantity} = ₱{subtotal}");
        }

        Console.WriteLine($"Grand Total: ₱{total}");

        double discount = 0;
        if (total >= 5000)
        {
            discount = total * 0.10;
            Console.WriteLine($"Discount: ₱{discount}");
        }

        Console.WriteLine($"Final Total: ₱{total - discount}");

        Console.WriteLine("\n--- UPDATED STOCK ---");
        foreach (var p in products)
            p.DisplayProduct();
    }
}
