using System;
using System.Globalization;
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
            var result = GetLatestProjects(context);
            Console.WriteLine(result);
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var project in projects)
            {
                var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                stringBuilder.AppendLine(project.Name);
                stringBuilder.AppendLine(project.Description);
                stringBuilder.AppendLine(startDate);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
