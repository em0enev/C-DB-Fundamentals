using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();

            Console.WriteLine(GetEmployee147(context));
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emp147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    empProjects = e.EmployeesProjects
                                .Select(x => x.Project.Name)
                                .OrderBy(x => x)
                                .ToList()
                })
                .ToList();

            foreach (var emp in emp147)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                foreach (var item in emp.empProjects)
                {
                    sb.AppendLine($"{item}");
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}
