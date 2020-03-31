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
            var result = GetEmployee147(context);
            Console.WriteLine(result);
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeFullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => new
                                            {
                                                ProjectName = p.Project.Name
                                            })
                                            .OrderBy(p => p.ProjectName)
                })
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var emp in employees)
            {
                stringBuilder.AppendLine($"{emp.EmployeeFullName} - {emp.JobTitle}");

                foreach (var project in emp.Projects)
                {
                    stringBuilder.AppendLine(project.ProjectName);
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
