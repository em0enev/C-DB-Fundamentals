using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace _06.AddingaNewAddressАndUpdatingEmployee
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            using (context)
            {
                Console.WriteLine(AddNewAddressToEmployee(context));
            }
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addres = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee nakov = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            nakov.Address = addres;

            context.SaveChanges();

            var employeeAdresses = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Select(a => a.Address.AddressText)
                .Take(10)
                .ToList();

            foreach (var address in employeeAdresses)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
