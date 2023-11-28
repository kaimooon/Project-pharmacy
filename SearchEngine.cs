using System;
using System.IO;

namespace midterm_project
{
    internal class SearchEngine
    {
        private string InventoryFilePath = "inventory.csv";
        private string TransactionFilePath = "transaction_history.csv";

        public void SearchProductInInventory(string partialProductName)
        {
            try
            {
                string[] inventoryLines = File.ReadAllLines(InventoryFilePath);

                bool productFound = false;

                foreach (string line in inventoryLines)
                {
                    string[] fields = line.Split(',');

                    if (fields.Length >= 4 && fields[1].ToLower().Contains(partialProductName.ToLower()))
                    {
                        Console.WriteLine($"Product found in inventory: {line}");
                        productFound = true;
                    }
                }

                if (!productFound)
                {
                    Console.WriteLine($"No products found in the inventory matching the search criteria: '{partialProductName}'.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Inventory file not found.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the inventory: {e.Message}");
            }
        }
    }
}
