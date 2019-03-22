﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core;
using MyApp.Core.Contracts;
using MyApp.Data;
using System;

namespace MyApp
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            IServiceProvider services = ConfigureServices();
            IEngine engine = new Engine(services);

            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection         
                .AddDbContext<MyAppContext>(db => db.UseSqlServer(@"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=MyAppDb;Integrated Security = true;"));

            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();

            serviceCollection.AddTransient<Mapper>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}