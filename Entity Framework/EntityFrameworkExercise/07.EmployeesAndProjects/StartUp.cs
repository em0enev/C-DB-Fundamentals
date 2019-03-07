using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace _07.EmployeesAndProjects
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(GetEmployeesInPeriod(context));
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    empInfo = $"{e.FirstName} {e.LastName}",
                    managerInfo = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    projectInfo = e.EmployeesProjects
                                   .Select(p => new
                                   {
                                       p.Project.Name,
                                       p.Project.StartDate,
                                       p.Project.EndDate
                                   })
                                   .ToList()
                })
                .Take(10)
                .ToList();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.empInfo} - Manager: {emp.managerInfo}");
                foreach (var proj in emp.projectInfo)
                {
                    var startDate = proj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.CreateSpecificCulture("en-Us"));

                    var endDate = proj.EndDate.HasValue ? proj.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.CreateSpecificCulture("en-Us")) :
                        "not finished";

                    sb.AppendLine($"--{proj.Name} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
