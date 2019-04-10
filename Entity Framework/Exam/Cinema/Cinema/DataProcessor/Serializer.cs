namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(m => m.Rating >= (double)rating)
                .Where(m => m.Projections.Select(p => p.Tickets.Sum(t => t.Price)).Sum() > 0)
                .Select(x => new
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("f2"),
                    TotalIncomes = x.Projections.Select(p => p.Tickets.Sum(t => t.Price)).Sum().ToString("f2"),

                    Customers = x.Projections
                    .SelectMany(p => p.Tickets.Where(c => c.Customer.Tickets.Any(z => z.CustomerId == c.CustomerId)))
                    .Select(t => t.Customer)
                    .Select(cu => new
                    {
                        cu.FirstName,
                        cu.LastName,
                        Balance = cu.Balance.ToString("f2")
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToList()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToList();
            ;
            var obj = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return obj;

        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));

            var custmers = context.Customers
                .Where(c => c.Age >= age)
                .Select(x => new ExportCustomerDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime = TimeSpan.FromTicks(x.Tickets.Select(t => t.Projection.Movie.Duration.Ticks).Sum()).ToString("hh\\:mm\\:ss")

                })
                .OrderByDescending(x => decimal.Parse(x.SpentMoney))
                .ThenBy(x => x.SpentTime)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("","")
            });

            using (StringWriter writer = new StringWriter(sb))
            {
                xs.Serialize(writer, custmers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}