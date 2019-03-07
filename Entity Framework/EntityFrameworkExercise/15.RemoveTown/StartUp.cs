using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace _15.RemoveTown
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addressesId = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .Select(a => a.AddressId)
                .ToList();

             var addresses = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToList();
            
            var people = context.Employees
                .Where(e => addressesId.Contains((int)e.AddressId))
                .ToList();

            var seattle = context.Towns.Where(t => t.Name == "Seattle").First();

            foreach (var p in people)
            {
                p.AddressId = null;
            }

            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(seattle);

            sb.AppendLine($"{addressesId.Count} addresses in Seattle were deleted");

            return sb.ToString().TrimEnd();
        }
    }
}
