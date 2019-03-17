using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App
{
    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            SeedUsers(context);

            SeedCreditCards(context);

            SeedBankAccounts(context);

            SeedPaymentMethod(context);
        }

        private static void SeedPaymentMethod(BillsPaymentSystemContext context)
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

            for (int i = 0; i < 8; i++)
            {
                var paymentMethod = new PaymentMethod()
                {
                    UserId = new Random().Next(1,5),
                    PaymentType =(PaymentType)new Random().Next(0,2),
                };

                if (i % 3 == 0)
                {
                    paymentMethod.CreditCardId = 1;
                    paymentMethod.BankAccountId= 1;
                }
                else if (i % 2 == 0)
                {
                    paymentMethod.CreditCardId = new Random().Next(1,5);
                }
                else
                {
                    paymentMethod.BankAccountId = new Random().Next(1, 5);
                }

                if (IsValid(paymentMethod))
                {
                    paymentMethods.Add(paymentMethod);
                }
            }

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext context)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();

            for (int i = 0; i < 8; i++)
            {
                var bankAcc = new BankAccount()
                {
                    Balance = new Random().Next(-200, 200),
                    BankName = "Bank" + i,
                    SWIFTCode = "SWIft" + i + 1
                };

                if (IsValid(bankAcc))
                {
                    bankAccounts.Add(bankAcc);
                }
            }

            context.BankAccounts.AddRange(bankAccounts);
            context.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext context)
        {
            var listOfCreditCards = new List<CreditCard>();

            for (int i = 0; i < 8; i++)
            {
                var creditCard = new CreditCard()
                {
                    Limit = new Random().Next(-10000, 25000),
                    MoneyOwed = new Random().Next(-10000, 25000),
                    ExpirationDate = DateTime.Now.AddDays(new Random().Next(-100,200))
                };

                if (IsValid(creditCard))
                {
                    listOfCreditCards.Add(creditCard);
                }
            }
            context.CreditCards.AddRange(listOfCreditCards);
            context.SaveChanges();
        }

        private static void SeedUsers(BillsPaymentSystemContext context)
        {
            string[] firstNames = { "Gosho", "Pesho", "Ivan", "Kiro", null };
            string[] lastNames = { "Goshev", "Peshev", "Ivanov", "Kirov", "Mirov" };
            string[] emails = { "Gosho@abv.bg", "Pesho@abv.bg", "Ivan@abv.bg", "Kiro@123.bg", "blablabla" };
            string[] passwords = { "Gosho@2bg", "Pesh321bg", "Ivan321312", "Kiro3232bg", "bl32la" };

            List<User> users = new List<User>();

            for (int i = 0; i < firstNames.Length; i++)
            {
                var user = new User()
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Email = emails[i],
                    Password = passwords[i]
                };

                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            ValidationContext validationContext = new ValidationContext(entity);

            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }
    }
}
