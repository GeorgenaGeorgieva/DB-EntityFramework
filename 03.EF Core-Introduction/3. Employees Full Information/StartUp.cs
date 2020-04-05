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
            Console.WriteLine(GetEmployeesFullInformation(context)); 
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (var empl in employees)
            {
                stringBuilder.AppendLine($"{empl.FirstName} {empl.LastName} " +
                    $"{empl.MiddleName} {empl.JobTitle} {empl.Salary:F2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
