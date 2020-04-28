namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using SoftUni.Data;
    using SoftUni.Models;
    
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = AddNewAddressToEmployee(context);
            Console.WriteLine(result);
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employeeNakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employeeNakov.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    AddressText = e.Address.AddressText
                })
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var e in employees)
            {
                stringBuilder.AppendLine(e.AddressText);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
