namespace SoftUni
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using SoftUni.Data;
    using SoftUni.Models;
    
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = GetEmployeesInPeriod(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(ep => ep.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 &&
                                                         p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmployeeFullName = e.FirstName + " " + e.LastName,
                    ManagerFullName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Project = e.EmployeesProjects.Select(e => new
                    {
                        e.Project.Name,
                        e.Project.StartDate,
                        e.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var emp in employees)
            {
                stringBuilder.AppendLine($"{emp.EmployeeFullName} - Manager: {emp.ManagerFullName}");

                foreach (var project in emp.Project)
                {
                    var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var endDate = project.EndDate == null
                        ? "not finished"
                        : ((DateTime)project.EndDate).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    stringBuilder.AppendLine($"--{project.Name} - {project.StartDate} - {project.EndDate}");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
