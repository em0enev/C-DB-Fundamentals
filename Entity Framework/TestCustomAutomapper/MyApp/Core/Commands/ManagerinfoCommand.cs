using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ManagerinfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ManagerinfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int managerId = int.Parse(inputArgs[0]);

            Employee manager = this.context.Employees
                .Include(m => m.ManagedEmployees)
                .FirstOrDefault(x => x.Id == managerId);

            var managerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.ManagedEmployees.Count}");
            foreach (var emp in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"    - {emp.FirstName} {emp.LastName} - ${emp.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
