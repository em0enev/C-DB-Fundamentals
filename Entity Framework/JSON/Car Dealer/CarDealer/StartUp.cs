using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //var suppliersJson = File.ReadAllText(@"C:\Users\emile\Desktop\Car Dealer\CarDealer\Datasets\suppliers.json");
            //var partsJson = File.ReadAllText(@"C:\Users\emile\Desktop\Car Dealer\CarDealer\Datasets\parts.json");
            //var carsJson = File.ReadAllText(@"C:\Users\emile\Desktop\Car Dealer\CarDealer\Datasets\cars.json");
            //var customersJson = File.ReadAllText(@"C:\Users\emile\Desktop\Car Dealer\CarDealer\Datasets\customers.json");
            //var salesJson = File.ReadAllText(@"C:\Users\emile\Desktop\Car Dealer\CarDealer\Datasets\sales.json");

            Console.WriteLine(GetSalesWithAppliedDiscount(context));

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            ;
            int lastSuplierId = context.Suppliers.LastOrDefault().Id;

            var validParts = new List<Part>();

            foreach (var part in parts)
            {
                if (part.SupplierId > lastSuplierId)
                {
                    continue;
                }
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length.ToString()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson);

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Length}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-Us")),
                    c.IsYoungDriver
                })
                .ToList();

            var customersJSON = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return customersJSON;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();

            string toyotaCarsJSON = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return toyotaCarsJSON;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            var suppliersJSON = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return suppliersJSON;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                            .Select(pc => new
                            {
                                pc.Part.Name,
                                Price = pc.Part.Price.ToString("0.00")
                            }
                            ).ToList()
                })
                .ToList();

            var carsWithPartsJSON = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);

            return carsWithPartsJSON;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var info = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount}",
                    price = $"{s.Car.PartCars.Sum(y => y.Part.Price):F2}",
                    priceWithDiscount = $"{s.Car.PartCars.Sum(y => y.Part.Price) - (s.Car.PartCars.Sum(y => y.Part.Price) * (s.Discount / 100)):F2}",
                })
                .ToList();

            var infoJSON = JsonConvert.SerializeObject(info, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,

            });

            return infoJSON;
        }
    }
}