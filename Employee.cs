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
            Console.WriteLine();
            Console.WriteLine("Press 'V' if you want to view the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'A' if you want to add item or stock in the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'R' if you want to remove stock in the inventory.");
            Console.WriteLine();
            Console.WriteLine("Press 'U' if you want to update the price of a product in the inventory");
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

        private void add()
        {
            try
            {
                string mainInventoryPath = "inventory.csv";
                string newStockPath = "newstock.csv";

                string[] mainInventoryLines = File.ReadAllLines(mainInventoryPath).Skip(1).ToArray(); // Skip the header line
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

                    // Extract the numeric part of the price value (remove non-numeric characters)
                    string priceString = newProductColumns[1].TrimStart('P', 'p');

                    if (double.TryParse(priceString, out double price) && int.TryParse(newProductColumns[2], out int stockQuantity))
                    {
                        bool productFound = false;

                        for (int i = 0; i < updatedMainInventory.Count; i++)
                        {
                            string[] columns = updatedMainInventory[i].Split(',');

                            if (columns.Length >= 4 && columns[1].Trim() == productName)
                            {
                                int currentStock = int.Parse(columns[3]); // Assuming quantity is at index 3
                                columns[3] = (currentStock + stockQuantity).ToString(); // Assuming quantity is at index 3
                                updatedMainInventory[i] = string.Join(",", columns);
                                productFound = true;
                                break;
                            }
                        }

                        if (!productFound)
                        {
                            // If the product doesn't exist, add a new row for the new product
                            string newProductLine = $"{GenerateProductId(updatedMainInventory)},{productName},0,{stockQuantity},{price}"; // Assuming price is at index 4
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

        private int GenerateProductId(IEnumerable<string> lines)
        {
            // Replace this with your logic to generate a new product ID
            // For simplicity, let's assume the product ID is one more than the maximum existing ID
            int maxId = lines.Select(line => int.Parse(line.Split(',')[0])).DefaultIfEmpty(0).Max();
            return maxId + 1;
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

        private string GenerateProductId(string[] inventoryLines)
        {
            // You can implement your own logic to generate a unique product ID
            // For simplicity, this example assumes that product IDs are numeric and increments from the last product ID in the inventory

            if (inventoryLines.Length == 0)
            {
                return "1"; // Start with product ID 1 if the inventory is empty
            }

            string lastProductId = inventoryLines.Last().Split(',')[0];
            if (int.TryParse(lastProductId, out int lastId))
            {
                return (lastId + 1).ToString();
            }

            // If parsing fails, return a default value
            return "1";
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
