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
            var result = GetDepartmentsWithMoreThan5Employees(context);
            Console.WriteLine(result);
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var departments = context.Departments
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName, 
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,   
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()
                })
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .ToList();

            foreach (var depart in departments)
            {
                stringBuilder.AppendLine($"{depart.Name} - {depart.ManagerFirstName} {depart.ManagerLastName}");

                foreach (var emp in depart.Employees)
                {
                    stringBuilder.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }
            
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
