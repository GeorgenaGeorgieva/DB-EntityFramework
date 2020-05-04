namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using SoftUni.Data;
    
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = IncreaseSalaries(context);
            Console.WriteLine(result);
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                         || e.Department.Name == "Tool Design"
                         || e.Department.Name == "Marketing "
                         || e.Department.Name == "Information Services")
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            var selectedEmployeesInformation = employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var employee in selectedEmployeesInformation)
            {
                stringBuilder.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
