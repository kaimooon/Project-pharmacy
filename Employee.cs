using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Failed to parse integer value from column '{columnName}' with input: {input}");
                return 0; // Or any default value that makes sense in your context
            }
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

            if (double.TryParse(numericPart, out double result))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Failed to parse double value from column '{columnName}' with input: {input}");
                return 0.0; // Or any default value that makes sense in your context
            }
        }

        private void DisplayLowStockWarning()
        {
            try
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
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }
        }

        private void add()
        {
            try
            {
                string mainInventoryPath = "inventory.csv";
                string newStockPath = "newstock.csv";

                string[] mainInventoryLines = File.ReadAllLines(mainInventoryPath); // Read all lines, including the header line
                string[] newStockLines = File.ReadAllLines(newStockPath).Skip(1).ToArray(); // Skip the header line

                List<string> updatedMainInventory = new List<string>(mainInventoryLines);

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
                                // Update the existing product's stock quantity
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
                            // Construct the new product line with a prefixed 'P' for the price without decimal points
                            string formattedPrice = $"P{price.ToString()}"; // Assuming price format: P + numeric value
                            string newProductLine = $"{GenerateProductId(updatedMainInventory)},{productName},{formattedPrice},{stockQuantity}";
                            updatedMainInventory.Add(newProductLine);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid format in the new stock entry: {newProduct}");
                        continue; // Skip the current iteration and move to the next one
                    }
                }

                // Write the updated content back to the file
                File.WriteAllLines(mainInventoryPath, updatedMainInventory);

                Console.WriteLine("Stock updated successfully!");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading/writing the file: {e.Message}");
            }
        }

        private string GenerateProductId(IEnumerable<string> lines)
        {
            try
            {
                string inventoryFilePath = "inventory.csv";

                if (!File.Exists(inventoryFilePath))
                {
                    return "1"; // If inventory file doesn't exist, start with product ID 1
                }

                string[] inventoryLines = File.ReadAllLines(inventoryFilePath);

                if (inventoryLines.Length == 0)
                {
                    return "1"; // If inventory file is empty, start with product ID 1
                }

                // Extracting the last product ID from the inventory
                string lastProductId = inventoryLines.Last().Split(',')[0];
                if (int.TryParse(lastProductId, out int lastId))
                {
                    return (lastId + 1).ToString(); // Return next available product ID
                }

                // Return a default value if the last product ID couldn't be parsed
                return "1";
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the inventory file: {e.Message}");
                return "1"; // Return a default value if an error occurs
            }
        }
        private double ExtractNumericValue(string input)
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

            // Attempt to parse the numeric part to double
            if (double.TryParse(numericPart, out double result))
            {
                return result;
            }
            else
            {
                // Handle the case where parsing fails
                Console.WriteLine($"Failed to parse numeric value from input: {input}");
                return 0.0; // Or any default value that makes sense in your context
            }
        }

        

        private void remove()
        {
            try
            {
                string mainInventoryPath = "inventory.csv";

                string[] mainInventoryLines = File.ReadAllLines(mainInventoryPath).Skip(1).ToArray(); // Skip the header line

                Console.Write("Enter the Product ID to remove: ");
                if (int.TryParse(Console.ReadLine(), out int productIdToRemove))
                {
                    bool productExists = mainInventoryLines.Any(line => int.Parse(line.Split(',')[0]) == productIdToRemove);

                    if (productExists)
                    {
                        Console.WriteLine($"Are you sure you want to delete product with ID {productIdToRemove} from the inventory? (Y/N)");

                        if (Console.ReadLine()?.ToUpper() == "Y")
                        {
                            mainInventoryLines = mainInventoryLines.Where(line => int.Parse(line.Split(',')[0]) != productIdToRemove).ToArray();

                            // Write the updated content back to the file
                            File.WriteAllLines(mainInventoryPath, mainInventoryLines);

                            Console.WriteLine($"Product with ID {productIdToRemove} removed successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Deletion canceled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Product with ID {productIdToRemove} not found in the inventory.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid Product ID.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading/writing the file: {e.Message}");
            }
        }
    }
}
