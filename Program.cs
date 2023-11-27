using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midterm_project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Are you a Customer or an Employee? (Type 'C' or 'E'): ");
            string user_Input = Console.ReadLine().ToUpper();
            Console.Clear();

            if (user_Input == "C")
            {
                Customer customer = new Customer();
            }
            else if (user_Input == "E")
            {
                Employee employee = new Employee();
            }
            else
            {
                Console.WriteLine("Invalid Input. Please type 'C' or 'E'.");
            }
        }
    }
}
