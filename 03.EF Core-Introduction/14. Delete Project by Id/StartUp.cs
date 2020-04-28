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
            var result = DeleteProjectById(context);
            Console.WriteLine(result);
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectForDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjectForDelete = context.EmployeesProjects
                .Where(p => p.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjectForDelete);
            context.Projects.Remove(projectForDelete);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name
                })
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var project in projects)
            {
                stringBuilder.AppendLine(project.Name);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
