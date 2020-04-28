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
            Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                stringBuilder.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
