using System;
using System.IO;

namespace midterm_project
{
    internal class Inventory
    {
        public void view()
        {
            Console.WriteLine("ItemID\tItemName\tPrice\tQuantity");

            try
            {
                string filePath = "inventory.csv";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');

                    // Assuming your CSV file has columns in the order of ItemID, ItemName, Quantity, and Price
                    Console.WriteLine($"{fields[0]}\t{fields[1]}\t{fields[2]}\t{fields[3]}");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }
        }
    }
}
