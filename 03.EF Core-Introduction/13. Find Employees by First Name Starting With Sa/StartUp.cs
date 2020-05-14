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
            var result = GetEmployeesByFirstNameStartingWithSa(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var empl in employees)
            {
                stringBuilder.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle} - (${empl.Salary})");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
