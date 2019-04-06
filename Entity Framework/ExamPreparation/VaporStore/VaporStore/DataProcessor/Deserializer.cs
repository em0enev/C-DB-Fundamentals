namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDTOs;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var games = new List<Game>();

            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                var developer = GetDevelopoer(context, gameDto.Developer);
                var genre = GetGenre(context, gameDto.Genre);

                game.Developer = developer;
                game.Genre = genre;

                foreach (var currentTag in gameDto.Tags)
                {
                    var tag = GetTag(context, currentTag);

                    game.GameTags.Add(new GameTag()
                    {
                        Game = game,
                        Tag = tag
                    });
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string currentTag)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Name == currentTag);

            if (tag == null)
            {
                tag = new Tag()
                {
                    Name = currentTag
                };

                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string genreDto)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Name == genreDto);

            if (genre == null)
            {
                genre = new Genre()
                {
                    Name = genreDto
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDevelopoer(VaporStoreDbContext context, string gameDtoDeveloper)
        {
            var developer = context.Developers.FirstOrDefault(x => x.Name == gameDtoDeveloper);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = gameDtoDeveloper
                };
                context.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var usersDto = JsonConvert.DeserializeObject<ImportUsersDto[]>(jsonString);

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var validCards = new List<Card>();

                foreach (var cardDto in userDto.Cards)
                {
                    if (IsValid(cardDto))
                    {
                        var card = new Card()
                        {
                            Number = cardDto.Number,
                            Cvc = cardDto.CVC,
                            Type = cardDto.Type
                        };
                        validCards.Add(card);
                    }
                }

                var user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = validCards
                };

                context.Cards.AddRange(validCards);
                context.Users.Add(user);
                context.SaveChanges();
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            ;
            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xs = new XmlSerializer(typeof(ImportParchaseDto[]), new XmlRootAttribute("Purchases"));

            var purchases = (ImportParchaseDto[])xs.Deserialize(new StringReader(xmlString));
            ;

            var validPurchases = new List<Purchase>();

            foreach (var pur in purchases)
            {
                var card = context.Cards.FirstOrDefault(x => x.Number == pur.CardNumber);
                var game = context.Games.FirstOrDefault(x => x.Name == pur.GameName);

                if (!IsValid(pur) || card == null || game == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var cardOwner = context.Cards
                    .Where(x => x.Number == pur.CardNumber)
                    .Select(x => new
                    {
                        x.User.Username
                    })
                    .First();

                var purchase = new Purchase()
                {
                    Type = pur.Type,
                    ProductKey = pur.ProductKey,
                    Date = DateTime.ParseExact(pur.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = card,
                    Game = game
                };
                validPurchases.Add(purchase);
                sb.AppendLine($"Imported {game.Name} for {cardOwner.Username}");
            }

            context.AddRange(validPurchases);
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