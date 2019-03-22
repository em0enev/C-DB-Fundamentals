using MyApp.Core.Commands.Contracts;
using MyApp.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace MyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        public string Read(string[] InputArgs)
        {
            string commandName = InputArgs[0] + Suffix;
            string[] commandParams = InputArgs.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly().GetTypes()
                .FirstOrDefault(x => x.Name == commandName);

            if (type == null)
            {
                throw new ArgumentNullException("invalid command");
            }

            var constructor = type.GetConstructors().FirstOrDefault();

            var constructorParams = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

            var services = constructorParams.Select(this.serviceProvider.GetService).ToArray();

            var command = (ICommand)Activator.CreateInstance(type, services);

            string result = command.Execute(commandParams);

            return result;
        }
    }
}
