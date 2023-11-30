using System;
using System.IO;
using System.Linq;

namespace midterm_project
{
    internal class SearchEngine
    {
        private string InventoryFilePath = "inventory.csv";
        private string TransactionFilePath = "transaction_history.csv";

        public void SearchProductInInventory(string partialProductName)
        {
            Console.Clear();
            string[] inventoryLines = File.ReadAllLines(InventoryFilePath).Skip(1).ToArray();

            Console.WriteLine("ItemID\tItemName\t\t\tPrice\tQuantity");

            bool productFound = false;

            foreach (string line in inventoryLines)
            {
                string[] fields = line.Split(',');

                if (fields.Length >= 4 && fields[1].ToLower().Contains(partialProductName.ToLower()))
                {
                    Console.WriteLine($"{fields[0]}\t{fields[1].PadRight(25)}\t{fields[2]}\t{fields[3]}");
                    productFound = true;
                }
            }

            if (!productFound)
            {
                Console.WriteLine($"No products found in the inventory matching the search criteria: '{partialProductName}'.");
            }
        }
    }
}
