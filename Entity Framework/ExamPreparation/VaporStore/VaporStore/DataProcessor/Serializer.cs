namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enum;
    using VaporStore.DataProcessor.ExportDTOs;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {

            var games = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Select(s => new ExportGamesDto
                    {
                        Id = s.Id,
                        Title = s.Name,
                        Developer = s.Developer.Name,
                        Tags = string.Join(", ", s.GameTags.Select(t => t.Tag.Name).ToList()),
                        Players = s.Purchases.Count()
                    })
                    .Where(ge => ge.Players > 0)
                    .OrderByDescending(ge => ge.Players)
                    .ThenBy(ge => ge.Id),
                    TotalPlayers = x.Games.Select(p => p.Purchases.Count()).Sum()
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();

            var obj = JsonConvert.SerializeObject(games, Newtonsoft.Json.Formatting.Indented);


            return obj.TrimEnd();
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ExportUserPurchasesByType[]), new XmlRootAttribute("Users"));

            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Where(p => p.Cards.SelectMany(x => x.Purchases).Any(s => s.Type == purchaseType))
                .Select(x => new ExportUserPurchasesByType
                {
                    Username = x.Username,

                    Purchases = x.Cards
                    .SelectMany(p => p.Purchases)
                    .Where(s => s.Type == purchaseType)
                    .Select(p => new ExportPurchaseDTO
                    {
                        CardNumber = p.Card.Number,
                        CVC = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToString(),
                        Game = new GameDTO
                        {
                            Title = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    })
                    .OrderBy(p => p.Date)
                    .ToArray(),

                    TotalSpent = x.Cards
                    .SelectMany(c => c.Purchases)
                    .Where(s => s.Type == purchaseType)
                    .Sum(p => p.Game.Price)
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

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
    }
}