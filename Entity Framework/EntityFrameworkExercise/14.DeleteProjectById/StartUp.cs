using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace _14.DeleteProjectById
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(DeleteProjectById(context));
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var project = context.Projects.Find(2);

            var forDelete = context.EmployeesProjects.Where(x => x.ProjectId == 2).ToList();

            foreach (var proj in forDelete)
            {
                context.EmployeesProjects.Remove(proj);
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            
            var projects = context.Projects
                .Select(p => new
                {
                    p.Name
                })
                .Take(10)
                .ToList();


            foreach (var proj in projects)
            {
                sb.AppendLine($"{proj.Name}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
