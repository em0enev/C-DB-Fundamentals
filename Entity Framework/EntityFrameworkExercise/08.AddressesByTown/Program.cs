using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace _08.AddressesByTowns
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();

            Console.WriteLine(GetAddressesByTown(context));
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var selectedAddresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    Text = a.AddressText,
                    Town = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .ToList();

            foreach (var address in selectedAddresses)
            {
                Console.WriteLine($"{address.Text}, {address.Town} - {address.EmployeesCount} employees");
            }

            return sb.ToString();
        }
    }
}
