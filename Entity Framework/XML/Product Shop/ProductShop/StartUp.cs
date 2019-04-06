using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new ProductShopProfile()));

            //var usersXML = File.ReadAllText(@"../../../Datasets/users.xml");
            //var productsXML = File.ReadAllText(@"../../../Datasets/products.xml");
            //var categoryXML = File.ReadAllText(@"../../../Datasets/categories.xml");
            //var categoryProdctXML = File.ReadAllText(@"../../../Datasets/categories-products.xml");

            using (ProductShopContext context = new ProductShopContext())
            {
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDto);

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {usersDto.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productDto = (ImportProductDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var products = Mapper.Map<Product[]>(productDto);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {productDto.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoriesDto[]), new XmlRootAttribute("Categories"));

            var categoryDto = (ImportCategoriesDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var categories = Mapper.Map<Category[]>(categoryDto);

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categoryDto.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProduct[]), new XmlRootAttribute("CategoryProducts"));

            var categoryDto = (ImportCategoryProduct[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = Mapper.Map<CategoryProduct[]>(categoryDto);

            var avalibleItems = new List<CategoryProduct>();
            var categoriesId = context.Categories.Select(c => c.Id).ToList();
            var productsId = context.Products.Select(p => p.Id).ToList();

            foreach (var cp in categoryProducts)
            {
                if (categoriesId.Contains(cp.CategoryId) && productsId.Contains(cp.ProductId))
                {
                    avalibleItems.Add(cp);
                }
            }

            context.CategoryProducts.AddRange(avalibleItems);
            context.SaveChanges();

            return $"Successfully imported {avalibleItems.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExportProductsInRange>), new XmlRootAttribute("Products"));

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ExportProductsInRange
                {
                    Name = p.Name,
                    Price = p.Price,
                    buyerFullName = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("","")
            });

            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, products, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<ExportSoldProduct>), new XmlRootAttribute("Users"));

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportSoldProduct
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(x => new ProductInfo
                    {
                        Name = x.Name,
                        Price = x.Price
                    })
                    .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("","")
            });

            using (StringWriter writer = new StringWriter(sb))
            {
                xs.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<ExportCategoriesByProductCount>), new XmlRootAttribute("Categories"));

            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ThenBy(c => c.CategoryProducts.Sum(cp => cp.Product.Price))
                .Select(c => new ExportCategoriesByProductCount
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("","")
            });

            using (StringWriter writer = new StringWriter(sb))
            {
                xs.Serialize(writer, categories, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ExportCustomUserProductDto), new XmlRootAttribute("Users"));

            var users = context
                .Users
                .Where(x => x.ProductsSold.Any())
                .Select(x => new ExportUserAndProductDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProductDto = new SoldProductDto
                    {
                        Count = x.ProductsSold.Count,
                        ProductDtos = x.ProductsSold.Select(p => new ProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }

                })
                .OrderByDescending(x => x.SoldProductDto.Count)
                .Take(10)
                .ToArray();

            var export = new ExportCustomUserProductDto
            {
                Count = context.Users.Count(x => x.ProductsSold.Any()),
                ExportUserAndProductDto = users
            };



            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("","")
            });

            using (StringWriter writer = new StringWriter(sb))
            {
                xs.Serialize(writer, export, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}