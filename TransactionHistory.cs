using System;
using System.IO;
using System.Linq;

namespace midterm_project
{
    internal class TransactionHistory
    {
        private string TransactionFilePath = "transaction_history.csv";

        public void RecordTransaction(string productName, int quantity, double totalPrice)
        {
            try
            {
                int orderId = GenerateOrderId();
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string transactionRecord = $"{orderId},{timeStamp},{productName},{quantity},{totalPrice}";

                using (StreamWriter writer = new StreamWriter(TransactionFilePath, true))
                {
                    writer.WriteLine(transactionRecord);
                }

                Console.WriteLine("Transaction recorded successfully!");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while writing the transaction history: {e.Message}");
            }
        }

        private int GenerateOrderId()
        {
            // Generating a unique order ID - here, assuming the order ID is one more than the maximum existing ID in the transaction history
            try
            {
                string[] transactionLines = File.ReadAllLines(TransactionFilePath);

                if (transactionLines.Length == 0)
                {
                    return 1; // Start with order ID 1 if transaction history is empty
                }

                int maxOrderId = transactionLines
                    .Select(line => int.Parse(line.Split(',')[0]))
                    .DefaultIfEmpty(0)
                    .Max();

                return maxOrderId + 1;
            }
            catch (FileNotFoundException)
            {
                // If the file doesn't exist, return 1 as the starting order ID
                return 1;
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the transaction history: {e.Message}");
                return 0; // Return 0 if an error occurs while reading the file
            }
        }
        public void SearchOrderByID(int orderId)
        {
            try
            {
                string[] transactionLines = File.ReadAllLines(TransactionFilePath);

                bool orderFound = false;

                foreach (string line in transactionLines)
                {
                    string[] fields = line.Split(',');

                    if (fields.Length >= 5 && int.TryParse(fields[0], out int currentOrderId) && currentOrderId == orderId)
                    {
                        Console.WriteLine($"Order Details: {line}");
                        orderFound = true;
                        break;
                    }
                }

                if (!orderFound)
                {
                    Console.WriteLine($"Order ID {orderId} not found in the transaction history.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Transaction history file not found.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the transaction history: {e.Message}");
            }
        }

        public void ViewAllTransactions()
        {
            try
            {
                string[] transactionLines = File.ReadAllLines(TransactionFilePath);

                if (transactionLines.Length > 0)
                {
                    Console.WriteLine("Transaction History:");
                    foreach (string line in transactionLines)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine("Transaction history is empty.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Transaction history file not found.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the transaction history: {e.Message}");
            }
        }
    }
}
