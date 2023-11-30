using System;
using System.IO;
using System.Linq;

namespace midterm_project
{
    internal class Inventory
    {
        public void view()
        {
            Console.WriteLine("ItemID\tItemName\t\t\tPrice\tQuantity");

            string filePath = "inventory.csv";
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');

                string formattedLine = $"{fields[0]}\t{fields[1].PadRight(25)}\t{fields[2]}\t{fields[3]}";
                Console.WriteLine(formattedLine);
            }
        }
    }
}
