using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetBirthdayCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] inputArgs)
        {
            int empId = int.Parse(inputArgs[0]);
            string[] dateArr = inputArgs[1].Split('-',StringSplitOptions.RemoveEmptyEntries).ToArray();
            int day = int.Parse(dateArr[0]);
            int month = int.Parse(dateArr[1]);
            int year = int.Parse(dateArr[2]);

            DateTime date = new DateTime(year, month, day);

            var emp = context.Employees.Find(2);

            if (emp == null)
            {
                throw new ArgumentNullException("Invalid employee id");
            }

            emp.Birthday = date;
            context.SaveChanges();

            return $"Birthday successfully set";
        }
    }
}
