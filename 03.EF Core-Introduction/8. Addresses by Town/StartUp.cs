using System;
using System.Linq;
using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = GetAddressesByTown(context);
            Console.WriteLine(result);
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                 .Select(a => new
                 {
                     a.AddressText,
                     TownName = a.Town.Name,
                     CountOfEmployees = a.Employees.Count,
                 })
                .OrderByDescending(a => a.CountOfEmployees)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var address in addresses)
            {
                stringBuilder.AppendLine($"{address.AddressText}, {address.TownName} - {address.CountOfEmployees} employees");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
