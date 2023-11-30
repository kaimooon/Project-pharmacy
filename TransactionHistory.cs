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
            int orderId = GenerateOrderId();
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string transactionRecord = $"{orderId},{timeStamp},{productName},{quantity},{totalPrice}";

            try
            {
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

        public void SearchOrderByID(int orderId)
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

        public void ViewAllTransactions()
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
    }
}
