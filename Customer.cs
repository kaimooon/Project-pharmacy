using System;
using System.IO;

namespace midterm_project
{
    internal class Customer
    {
        public Customer()
        {
            Console.WriteLine("Welcome Customer!");
            Console.WriteLine();
            Console.WriteLine("Press 'V' to view the available stock.");
            Console.WriteLine();
            Console.WriteLine("Press 'B' to buy something from the store.");
            Console.WriteLine();
            Console.WriteLine("Press 'E' to exit.");
            string user_Input = Console.ReadLine().ToUpper();

            if (user_Input == "V")
            {
                Inventory inventory = new Inventory();
                inventory.view();
            }
            else if (user_Input == "B")
            {
                Console.WriteLine("Welcome Customer");

                try
                {
                    Inventory inventory = new Inventory();
                    inventory.view(); // Show available items

                    Console.Write("Enter the ItemID you want to purchase: ");
                    int itemId;
                    while (!int.TryParse(Console.ReadLine(), out itemId))
                    {
                        Console.WriteLine("Invalid ItemID. Please enter a valid ItemID: ");
                    }

                    Console.Write("Enter the quantity you want to purchase: ");
                    int quantity;
                    while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity. Please enter a valid quantity: ");
                    }

                    string selectedProduct = GetProductNameById(itemId); // Retrieve product name from ItemID
                    double totalPrice = CalculateTotalPrice(selectedProduct, quantity); // Calculate total price

                    Console.WriteLine($"Selected Product: {selectedProduct}");
                    Console.WriteLine($"Quantity: {quantity}");
                    Console.WriteLine($"Total Price: {totalPrice}");

                    Console.Write("Confirm purchase? (Y/N): ");
                    string confirmation = Console.ReadLine()?.ToUpper();

                    if (confirmation == "Y")
                    {
                        TransactionHistory transactionHistory = new TransactionHistory();
                        transactionHistory.RecordTransaction(selectedProduct, quantity, totalPrice);
                        Console.WriteLine("Purchase successful!");
                    }
                    else
                    {
                        Console.WriteLine("Purchase canceled.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else if (user_Input == "E")
            {
                Console.WriteLine("Bye");
                Console.ReadKey();
            }
        }
        // Helper method to retrieve product name by ItemID from the inventory
        private string GetProductNameById(int itemId)
        {
            try
            {
                string filePath = "inventory.csv";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');

                    if (int.TryParse(fields[0], out int currentItemId) && currentItemId == itemId)
                    {
                        return fields[1]; // Return product name if ItemID matches
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }

            return string.Empty; // Return an empty string if ItemID is not found
        }

        // Helper method to calculate total price
        private double CalculateTotalPrice(string productName, int quantity)
        {
            try
            {
                string filePath = "inventory.csv";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');

                    if (fields[1] == productName)
                    {
                        // Extracting the numeric part from the price string (e.g., 'P10' -> '10')
                        string priceString = fields[2].Substring(1); // Assuming the price format is always 'P' followed by the numeric value
                        if (double.TryParse(priceString, out double price))
                        {
                            return price * quantity; // Calculate total price
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }

            return 0.0; // Return 0.0 if the calculation fails
        }
    }
}
