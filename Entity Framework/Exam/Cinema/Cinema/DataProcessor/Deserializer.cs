namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var movies = JsonConvert.DeserializeObject<Movie[]>(jsonString);
            var validMovies = new List<Movie>();

            foreach (var m in movies)
            {
                if (!IsValid(m))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validMovies.Add(m);
                sb.AppendLine($"Successfully imported {m.Title} with genre {m.Genre} and rating {m.Rating:f2}!");
            }

            context.AddRange(validMovies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var validHalls = new List<Hall>();
            var seats = new List<Seat>();

            var halls = JsonConvert.DeserializeObject<HallDto[]>(jsonString);


            foreach (var hall in halls)
            {
                if (!IsValid(hall))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentHallSeats = new List<Seat>();

                for (int i = 0; i < hall.Seats; i++)
                {
                    var seat = new Seat();
                    currentHallSeats.Add(seat);
                }
                context.Seats.AddRange(currentHallSeats);


                var validHall = new Hall()
                {
                    Name = hall.Name,
                    Is3D = hall.Is3D,
                    Is4Dx = hall.Is4Dx,
                    Seats = currentHallSeats
                };

                string msg = string.Empty;

                if (hall.Is3D && !hall.Is4Dx)
                {
                    msg = "3D";
                }
                else if (!hall.Is3D && hall.Is4Dx)
                {
                    msg = "4Dx";
                }
                else if (!hall.Is3D && !hall.Is4Dx)
                {
                    msg = "Normal";
                }
                else
                {
                    msg = "4Dx/3D";
                }

                validHalls.Add(validHall);

                sb.AppendLine($"Successfully imported {validHall.Name}({msg}) with {validHall.Seats.Count} seats!");
            }

            context.Halls.AddRange(validHalls);
            context.SaveChanges();
            ;

            return sb.ToString().TrimEnd(); ;

        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xs = new XmlSerializer(typeof(ProjectionDTO[]), new XmlRootAttribute("Projections"));
            var projections = (ProjectionDTO[])xs.Deserialize(new StringReader(xmlString));

            var validProjections = new List<Projection>();

            foreach (var proj in projections)
            {
                var movie = context.Movies.Find(proj.MovieId);
                var hall = context.Halls.Find(proj.HallId);
                var datetimeDTO = DateTime.ParseExact(proj.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                var formatedDate = datetimeDTO.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                var sss = DateTime.ParseExact(formatedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (!IsValid(proj) || movie == null || hall == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection()
                {
                    Movie = movie,
                    Hall = hall,
                    DateTime = sss
                };

                validProjections.Add(projection);
                sb.AppendLine($"Successfully imported projection {movie.Title} on {sss.ToString("MM/dd/yyyy")}!");
            }

            context.Projections.AddRange(validProjections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xs = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var customers = (ImportCustomerDto[])xs.Deserialize(new StringReader(xmlString));

            var validCustomers = new List<Customer>();
            var validTickets = new List<Ticket>();

            foreach (var person in customers)
            {
                var currentTickets = new List<Ticket>();

                if (!IsValid(person))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age,
                    Balance = person.Balance
                };

                var currentCustomerTickets = new List<Ticket>();
                foreach (var tick in person.Tickets)
                {
                    var projection = context.Projections.Find(tick.ProjectionId);

                    if (projection == null)
                    {
                        continue;
                    }

                    var ticket = new Ticket()
                    {
                        Price = tick.Price,
                        Projection = projection
                    };

                    currentCustomerTickets.Add(ticket);
                }

                sb.AppendLine($"Successfully imported customer {customer.FirstName} {customer.LastName} with bought tickets: {currentCustomerTickets.Count}!");
                validTickets.AddRange(currentCustomerTickets);

                customer.Tickets = currentCustomerTickets;

                context.Customers.Add(customer);
                context.AddRange(currentCustomerTickets);
                context.SaveChanges();
            }

           // context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            ;
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}