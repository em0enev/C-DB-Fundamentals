using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            // var usersJson = File.ReadAllText(@"C:\Users\emile\Desktop\User Product\ProductShop\Datasets\categories-products.json");

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            List<User> validUsers = new List<User>();

            foreach (var user in users)
            {
                if (user.IsValid())
                {
                    validUsers.Add(user);
                }
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            List<Product> validProducts = new List<Product>();

            foreach (var product in products)
            {
                if (product.IsValid())
                {
                    validProducts.Add(product);
                }
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            List<Category> validcategories = new List<Category>();

            foreach (var category in categories)
            {
                if (category.IsValid())
                {
                    validcategories.Add(category);
                }
            }

            context.Categories.AddRange(validcategories);
            context.SaveChanges();

            return $"Successfully imported {validcategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ProductDTO
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                })
                .ToList();

            var productsJSON = JsonConvert.SerializeObject(products, Formatting.Indented);

            ;
            return productsJSON;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(p => p.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(s => new
                    {
                        name = s.Name,
                        price = s.Price,
                        buyerFirstName = s.Buyer.FirstName,
                        buyerLastName = s.Buyer.LastName
                    }).ToList()
                })
                .ToList();

            var usersJSON = JsonConvert.SerializeObject(users);
            ;
            return usersJSON.TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = $"{ c.CategoryProducts.Average(x => x.Product.Price):f2}",
                    totalRevenue = $"{ c.CategoryProducts.Sum(x => x.Product.Price):f2}"
                }).ToList();

            var categoriesJSON = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return categoriesJSON;

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var people = context.Users
                .Where(u => u.ProductsSold.Any(x => x.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Where(ps => ps.Buyer != null).Count())
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Count(ps => ps.Buyer != null),
                        products = x.ProductsSold
                                    .Where(ps => ps.Buyer != null)
                                    .Select(p => new
                                    {
                                        name = p.Name,
                                        price = p.Price
                                    }).ToList() 
                    }
                })
                .ToList();

            var result = new
            {
                usersCount = people.Count(),
                users = people
            };

            var usersJSON = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return usersJSON;
        }
    }
}