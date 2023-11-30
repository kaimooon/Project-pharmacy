using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace midterm_project
{
    internal class Employee
    {
        public Employee()
        {
            Console.WriteLine("Welcome Employee!");
            DisplayLowStockWarning();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press 'V' if you want to view the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'A' if you want to add item or stock in the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'R' if you want to remove stock in the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'T' if you want to view the transaction history.");
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
            else if (user_Input == "A")
            {
                add();
            }
            else if (user_Input == "R")
            {
                remove();
            }
            else if (user_Input == "T")
            {
                TransactionHistory history = new TransactionHistory();
                history.ViewAllTransactions();
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
                return;
            }
            else
            {
                Console.WriteLine("Invalid Input. Please Input 'V', 'A', 'R', 'T', or 'E'.");
            }
        }

        private int ParseIntValue(string input, string columnName)
        {
            return int.TryParse(input, out int result) ? result : 0;
        }

        private double ParseDoubleValue(string input, string columnName)
        {
            StringBuilder numericPartBuilder = new StringBuilder();

            foreach (char character in input)
            {
                if (char.IsDigit(character) || character == '.' || character == '-')
                {
                    numericPartBuilder.Append(character);
                }
            }

            string numericPart = numericPartBuilder.ToString();

            return double.TryParse(numericPart, out double result) ? result : 0.0;
        }

        private void DisplayLowStockWarning()
        {
            string filePath = "inventory.csv";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');

                if (int.TryParse(fields[3], out int quantity) && quantity <= 5)
                {
                    string itemName = fields[1];
                    Console.WriteLine($"WARNING: LOW STOCK FOR {itemName.ToUpper()}!");
                }
            }
        }

        private void add()
        {
            string mainInventoryPath = "inventory.csv";
            string newStockPath = "newstock.csv";

            string[] mainInventoryLines = File.ReadAllLines(mainInventoryPath);
            string[] newStockLines = File.ReadAllLines(newStockPath).Skip(1).ToArray(); // Skip the header line

            List<string> updatedMainInventory = new List<string>(mainInventoryLines);

            int lastProductId = 0;

            foreach (string line in updatedMainInventory)
            {
                string[] columns = line.Split(',');
                if (columns.Length > 0 && int.TryParse(columns[0], out int currentId) && currentId > lastProductId)
                {
                    lastProductId = currentId;
                }
            }

            foreach (string newProduct in newStockLines)
            {
                string[] newProductColumns = newProduct.Split(',');

                if (newProductColumns.Length < 3)
                {
                    Console.WriteLine($"Invalid format in the new stock entry: {newProduct}");
                    continue; // Skip the current iteration and move to the next one
                }

                string productName = newProductColumns[0];
                string priceString = newProductColumns[1].Trim(); // Trim spaces

                if (int.TryParse(priceString.Substring(1), out int price) && int.TryParse(newProductColumns[2], out int stockQuantity))
                {
                    bool productFound = false;

                    for (int i = 0; i < updatedMainInventory.Count; i++)
                    {
                        string[] columns = updatedMainInventory[i].Split(',');

                        if (columns.Length >= 4 && columns[1].Trim() == productName)
                        {
                            int currentStock;
                            if (int.TryParse(columns[3], out currentStock))
                            {
                                currentStock += stockQuantity;
                                columns[3] = currentStock.ToString();
                                updatedMainInventory[i] = string.Join(",", columns);
                                productFound = true;
                                break;
                            }
                        }
                    }

                    if (!productFound)
                    {
                        lastProductId++;
                        string formattedPrice = $"P{price.ToString()}";
                        string newProductLine = $"{lastProductId},{productName},{formattedPrice},{stockQuantity}";
                        updatedMainInventory.Add(newProductLine);
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid format in the new stock entry: {newProduct}");
                    continue; // Skips the current iteration and move to the next one
                }
            }

            File.WriteAllLines(mainInventoryPath, updatedMainInventory);

            Console.WriteLine("Stock updated successfully!");
        }

        private void remove()
        {
            string mainInventoryPath = "inventory.csv";

            string[] mainInventoryLines = File.ReadAllLines(mainInventoryPath).Skip(1).ToArray(); // Skips the header line

            Console.Write("Enter the Product ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int productIdToRemove))
            {
                bool productExists = false;

                for (int i = 0; i < mainInventoryLines.Length; i++)
                {
                    string[] columns = mainInventoryLines[i].Split(',');

                    if (columns.Length > 0)
                    {
                        int currentId;
                        if (int.TryParse(columns[0], out currentId) && currentId == productIdToRemove)
                        {
                            productExists = true;

                            Console.WriteLine($"Are you sure you want to delete product with ID {productIdToRemove} from the inventory? (Y/N)");
                            if (Console.ReadLine()?.ToUpper() == "Y")
                            {
                                List<string> updatedInventory = new List<string>(mainInventoryLines);
                                updatedInventory.RemoveAt(i);

                                File.WriteAllLines(mainInventoryPath, updatedInventory);

                                Console.WriteLine($"Product with ID {productIdToRemove} removed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Deletion canceled.");
                            }

                            break;
                        }
                    }
                }

                if (!productExists)
                {
                    Console.WriteLine($"Product with ID {productIdToRemove} not found in the inventory.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Product ID.");
            }
        }
    }
}
