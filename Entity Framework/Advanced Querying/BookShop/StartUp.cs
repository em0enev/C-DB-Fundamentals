namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            BookShopContext context = new BookShopContext();

            using (context)
            {
                IncreasePrices(context);

            }

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var title in books)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new { b.Title, b.BookId })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {

                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            List<string> categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            var bookCategories = context.Books
                .Where(x => x.BookCategories
                    .Select(a => a.Category.Name.ToLower())
                    .Intersect(categories)
                    .Any())
                    .Select(x => x.Title)
                    .OrderBy(x => x);
            ;

            return String.Join(Environment.NewLine, bookCategories);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var inputSplit = date.Split("-", StringSplitOptions.RemoveEmptyEntries).ToArray();

            int day = int.Parse(inputSplit[0]);
            int month = int.Parse(inputSplit[1]);
            int year = int.Parse(inputSplit[2]);

            DateTime finalDate = new DateTime(year,month,day);

            var books = context.Books
                .Where(b => b.ReleaseDate < finalDate)
                .Select(b => new
                {
                    b.Title,
                    type = b.EditionType.ToString(),
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.type} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var bookTitle = context.Books
                 .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                 .Select(b => b.Title)
                 .OrderBy(b => b);

            foreach (var title in bookTitle)
            {
                sb.AppendLine($"{title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var booksWithAuthors = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName,
                    b.BookId
                })
                .Where(b => b.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId);

            foreach (var book in booksWithAuthors)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var countBooks = context.Books
                .Where(b => b.Title.Count() > lengthCheck)
                .Count();

            return countBooks;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    copiesCount = a.Books.Select(x => x.Copies).Sum()
                })
                .OrderByDescending(x => x.copiesCount)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName} - {author.copiesCount}");
            }

            return sb.ToString().TrimEnd() ;
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var profit = context.Categories
                .Select(c => new
                {
                    c.Name,
                    profit = c.CategoryBooks.Select(b => b.Book.Price * b.Book.Copies).Sum()
                })
                .OrderByDescending(x => x.profit)
                .ToList();

            foreach (var item in profit)
            {
                sb.AppendLine($"{item.Name} ${item.profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var recentBooks = context.Categories
                .Select(c => new
                {
                    c.Name,
                    books = c.CategoryBooks
                             .Select(b => new
                             {
                                 b.Book.Title,
                                 b.Book.ReleaseDate
                             })
                             .OrderByDescending(b => b.ReleaseDate)
                             .Take(3)
                             .ToList()
                })
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var category in recentBooks)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksForRomove = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(booksForRomove);
            context.SaveChanges();

            return booksForRomove.Count();
        }
    }
}
