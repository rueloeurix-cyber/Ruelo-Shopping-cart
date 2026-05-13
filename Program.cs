using System;
using System.Linq;

namespace Get_and_Set
{

    class Program
    {
        static int receiptCounter = 1;

        static void Main()
        {
            Product[] products = new Product[]
            {
            new Product(1, "Sofa", 30000, 8, "Furniture"),
            new Product(2, "Cushion", 500, 10, "Furniture"),
            new Product(3, "Microwave", 1500, 7, "Electronics"),
            new Product(4, "Mini fridge", 2000, 7, "Electronics"),
            new Product(5, "Black T-shirt", 300, 15, "Clothing"),
            new Product(6, "Chicken Hotdog", 50, 20, "Food"),
            new Product(7, "Frozen French fries", 220, 8, "Food"),
            new Product(8, "White T-shirt", 280, 20, "Clothing")
            };

            CartItem[] cart = new CartItem[10];
            int cartCount = 0;

            Order[] history = new Order[10];
            int historyCount = 0;

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Item to Cart");
                Console.WriteLine("3. View Cart");
                Console.WriteLine("4. Remove Item");
                Console.WriteLine("5. Update Quantity");
                Console.WriteLine("6. Clear Cart");
                Console.WriteLine("7. Search Product");
                Console.WriteLine("8. Checkout");
                Console.WriteLine("9. View Order History");
                Console.WriteLine("10. Exit");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        foreach (var p in products)
                            p.DisplayProduct();
                        break;

                    case "2":
                        bool addingItems = true;

                        while (addingItems)
                        {
                            Console.WriteLine("\nSelect Category:");
                            Console.WriteLine("1. Food");
                            Console.WriteLine("2. Electronics");
                            Console.WriteLine("3. Clothing");
                            Console.WriteLine("4. Furniture");
                            Console.WriteLine("5. All");
                            Console.WriteLine("0. Back to Menu");

                            Console.Write("Enter choice: ");
                            string catChoice = Console.ReadLine();

                            string selectedCategory = "";

                            switch (catChoice)
                            {
                                case "1": selectedCategory = "Food"; break;
                                case "2": selectedCategory = "Electronics"; break;
                                case "3": selectedCategory = "Clothing"; break;
                                case "4": selectedCategory = "Furniture"; break;
                                case "5": selectedCategory = "All"; break;
                                case "0":
                                    addingItems = false;
                                    continue;
                                default:
                                    Console.WriteLine("Invalid category.");
                                    continue;
                            }

                            Console.WriteLine("\nAvailable Products:");
                            bool hasItems = false;

                            foreach (var p in products)
                            {
                                if (selectedCategory == "All" || p.Category == selectedCategory)
                                {
                                    p.DisplayProduct();
                                    hasItems = true;
                                }
                            }

                            if (!hasItems)
                            {
                                Console.WriteLine("No products in this category.");
                                continue;
                            }

                            Console.Write("Enter product ID (0 to go back): ");
                            if (!int.TryParse(Console.ReadLine(), out int productChoice) || productChoice < 0)
                            {
                                Console.WriteLine("Invalid choice.");
                                continue;
                            }

                            if (productChoice == 0)
                                continue;

                            Product selected = null;

                            foreach (var p in products)
                            {
                                if (p.Id == productChoice)
                                {
                                    selected = p;
                                    break;
                                }
                            }

                            if (selected == null)
                            {
                                Console.WriteLine("Product not found.");
                                continue;
                            }

                            Console.Write("Enter quantity: ");
                            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                            {
                                Console.WriteLine("Invalid quantity.");
                                continue;
                            }

                            if (!selected.HasEnoughStock(qty))
                            {
                                Console.WriteLine($"Only {selected.RemainingStock} left.");
                                continue;
                            }

                            bool found = false;
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == selected.Id)
                                {
                                    cart[i].Quantity += qty;
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
                                cart[cartCount++] = new CartItem(selected, qty);
                            }

                            selected.DeductStock(qty);
                            Console.WriteLine("Added to cart!");


                            while (true)
                            {
                                Console.Write("\nAdd another item? (Y/N): ");
                                string again = Console.ReadLine().ToUpper();

                                if (again == "Y")
                                {
                                    break;
                                }
                                else if (again == "N")
                                {
                                    addingItems = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter Y or N only.");
                                }
                            }
                        }
                        break;

                    case "3":
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                            break;
                        }
                        Console.WriteLine("\n--- CART ---");
                        double total = 0;
                        for (int i = 0; i < cartCount; i++)
                        {
                            double sub = cart[i].GetSubtotal();
                            total += sub;
                            Console.WriteLine($"{cart[i].Product.Name} x{cart[i].Quantity} = ₱{sub}");
                        }
                        Console.WriteLine($"Total: ₱{total}");
                        break;

                    case "4":
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                            break;
                        }
                        Console.Write("Enter product ID to remove: ");
                        if (!int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        bool removed = false;
                        for (int i = 0; i < cartCount; i++)
                        {
                            if (cart[i].Product.Id == removeId)
                            {
                                cart[i].Product.RemainingStock += cart[i].Quantity;

                                for (int j = i; j < cartCount - 1; j++)
                                    cart[j] = cart[j + 1];

                                cartCount--;
                                Console.WriteLine("Item removed.");
                                removed = true;
                                break;
                            }
                        }
                        if (!removed)
                            Console.WriteLine("Product ID not found in cart.");
                        break;

                    case "5":
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                            break;
                        }
                        Console.Write("Enter product ID to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        bool updated = false;
                        for (int i = 0; i < cartCount; i++)
                        {
                            if (cart[i].Product.Id == updateId)
                            {
                                Console.Write("New quantity: ");
                                if (!int.TryParse(Console.ReadLine(), out int newQty) || newQty <= 0)
                                {
                                    Console.WriteLine("Invalid quantity.");
                                    break;
                                }

                                int diff = newQty - cart[i].Quantity;

                                if (diff > 0 && !cart[i].Product.HasEnoughStock(diff))
                                {
                                    Console.WriteLine("Not enough stock.");
                                    break;
                                }

                                cart[i].Product.RemainingStock -= diff;
                                cart[i].Quantity = newQty;

                                Console.WriteLine("Quantity updated.");
                                updated = true;
                                break;
                            }
                        }
                        if (!updated)
                            Console.WriteLine("Product ID not found in cart.");
                        break;

                    case "6":
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Your cart is already empty.");
                            break;
                        }


                        while (true)
                        {
                            Console.Write("Are you sure you want to clear the cart? (Y/N): ");
                            string confirm = Console.ReadLine().ToUpper();

                            if (confirm == "Y")
                            {
                                for (int i = 0; i < cartCount; i++)
                                    cart[i].Product.RemainingStock += cart[i].Quantity;

                                cartCount = 0;
                                Console.WriteLine("Cart cleared.");
                                break;
                            }
                            else if (confirm == "N")
                            {
                                Console.WriteLine("Cart was not cleared.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter Y or N only.");
                            }
                        }
                        break;

                    case "7":
                        Console.Write("Enter product name to search: ");
                        string keyword = Console.ReadLine().ToLower();

                        bool foundProduct = false;

                        foreach (var p in products)
                        {
                            if (p.Name.ToLower().Contains(keyword))
                            {
                                p.DisplayProduct();
                                foundProduct = true;
                            }
                        }

                        if (!foundProduct)
                            Console.WriteLine("No products found.");
                        break;

                    case "8":

                        if (cartCount == 0)
                        {
                            Console.WriteLine("Your cart is empty. Add items before checking out.");
                            break;
                        }

                        double grandTotal = 0;
                        Console.WriteLine("\n--- RECEIPT ---");

                        for (int i = 0; i < cartCount; i++)
                        {
                            double sub = cart[i].GetSubtotal();
                            grandTotal += sub;
                            Console.WriteLine($"{cart[i].Product.Name} x{cart[i].Quantity} = ₱{sub}");
                        }

                        double discount = grandTotal >= 5000 ? grandTotal * 0.10 : 0;
                        double finalTotal = grandTotal - discount;

                        Console.WriteLine($"Total: ₱{grandTotal}");
                        Console.WriteLine($"Discount: ₱{discount}");
                        Console.WriteLine($"Final Total: ₱{finalTotal}");

                        double payment;
                        while (true)
                        {
                            Console.Write("Enter payment: ₱");
                            if (!double.TryParse(Console.ReadLine(), out payment))
                            {
                                Console.WriteLine("Invalid payment. Please enter a numeric value.");
                                continue;
                            }
                            if (payment < finalTotal)
                            {
                                Console.WriteLine("Insufficient payment. Please enter a higher amount.");
                                continue;
                            }
                            break;
                        }

                        double change = payment - finalTotal;


                        string formattedDate = DateTime.Now.ToString("MMMM dd, yyyy h:mm tt");

                        Console.WriteLine($"\nReceipt No: {receiptCounter:0000}");
                        Console.WriteLine($"Date: {formattedDate}");
                        Console.WriteLine($"Payment: ₱{payment}");
                        Console.WriteLine($"Change: ₱{change}");

                        if (historyCount < history.Length)
                            history[historyCount++] = new Order(receiptCounter, finalTotal);
                        else
                            Console.WriteLine("Order history is full.");

                        receiptCounter++;

                        Console.WriteLine("\nLOW STOCK ALERT:");
                        bool anyLowStock = false;
                        foreach (var p in products)
                        {
                            if (p.RemainingStock <= 5)
                            {
                                Console.WriteLine($"{p.Name} has only {p.RemainingStock} stock left.");
                                anyLowStock = true;
                            }
                        }
                        if (!anyLowStock)
                            Console.WriteLine("All products have sufficient stock.");

                        cartCount = 0;
                        break;

                    case "9":
                        Console.WriteLine("\n--- ORDER HISTORY ---");
                        if (historyCount == 0)
                        {
                            Console.WriteLine("No orders yet.");
                            break;
                        }
                        for (int i = 0; i < historyCount; i++)
                        {

                            Console.WriteLine($"Receipt #{history[i].ReceiptNo:0000} - Final Total: ₱{history[i].FinalTotal}");
                        }
                        break;

                    case "10":
                        running = false;
                        Console.WriteLine("Thank you for shopping! Goodbye.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please choose a number from 1 to 10.");
                        break;
                }
            }
        }
    }
}
