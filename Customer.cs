using System;
using System.Collections.Generic;
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
            Console.WriteLine("Press 'S' if you want to search for an item in the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'E' to exit.");
            string user_Input = Console.ReadLine().ToUpper();
            Console.Clear();

            if (user_Input == "V")
            {
                Inventory inventory = new Inventory();
                inventory.view();
            }
            else if (user_Input == "B")
            {
                Console.WriteLine("Welcome Customer");

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
                    UpdateInventoryAfterPurchase(itemId, quantity); // Update inventory after purchase
                    Console.WriteLine("Purchase successful!");
                }
                else
                {
                    Console.WriteLine("Purchase canceled.");
                }
            }
            else if (user_Input == "S")
            {
                Console.WriteLine("Type in the name of the product: ");
                string partialName = Console.ReadLine();

                SearchEngine searchEngine = new SearchEngine();
                searchEngine.SearchProductInInventory(partialName);
            }
            else if (user_Input == "E")
            {
                Console.WriteLine("Bye");
                Console.ReadKey();
            }
        }

        private void UpdateInventoryAfterPurchase(int itemId, int purchasedQuantity)
        {
            string filePath = "inventory.csv";
            string[] lines = File.ReadAllLines(filePath);

            List<string> updatedLines = new List<string>();

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');

                if (int.TryParse(fields[0], out int currentItemId) && currentItemId == itemId)
                {
                    int currentQuantity;
                    if (int.TryParse(fields[3], out currentQuantity))
                    {
                        // Decrease the quantity by the purchased quantity
                        currentQuantity -= purchasedQuantity;
                        fields[3] = currentQuantity.ToString();
                    }
                }

                // Add the modified or unmodified line to the updated list
                updatedLines.Add(string.Join(",", fields));
            }

            // Write the updated content back to the file
            File.WriteAllLines(filePath, updatedLines);

            Console.WriteLine("Inventory updated after purchase!");
        }

        private string GetProductNameById(int itemId)
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

            return string.Empty; // Return an empty string if ItemID is not found
        }

        private double CalculateTotalPrice(string productName, int quantity)
        {
            string filePath = "inventory.csv";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');

                if (fields[1] == productName)
                {
                    // Extracting the numeric part from the price string (e.g., 'P10' -> '10')
                    double price = 0.0;
                    bool isNumericPart = false;

                    foreach (char character in fields[2])
                    {
                        if (char.IsDigit(character) || character == '.')
                        {
                            isNumericPart = true;
                            price = price * 10 + (character - '0');
                        }
                        else if (isNumericPart)
                        {
                            break; // Stop when non-numeric characters appear after the numeric part
                        }
                    }

                    return price * quantity; // Calculate total price
                }
            }

            return 0.0; // Return 0.0 if the calculation fails
        }
    }
}
