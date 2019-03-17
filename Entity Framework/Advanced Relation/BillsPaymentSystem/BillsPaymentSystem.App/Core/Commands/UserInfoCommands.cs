using BillsPaymentSystem.App.Core.Commands.Contracts;
using BillsPaymentSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class UserInfoCommands : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public UserInfoCommands(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args, BillsPaymentSystemContext context)
        {
            int userId = int.Parse(args[0]);

            var user = this.context.Users
                .FirstOrDefault(x => x.UserId == userId);

            var paymentMethods = context.Users
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    bankAcc = x.PaymentMethods.Where(b=> b.BankAccount !=null).Select(s => s.BankAccount).ToList(),
                    creditCardAacc = x.PaymentMethods.Where(b => b.BankAccount != null).Select(s => s.CreditCard).ToList()
                })
                .ToList();

            if (user == null)
            {
                throw new ArgumentNullException("User not found!");
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User: {user.FirstName} {user.LastName}");

            foreach (var acc in paymentMethods)
            {
                sb.AppendLine($"Bank Accounts: {acc.bankAcc.Count}");
                foreach (var bankacc in acc.bankAcc)
                {
                    sb.AppendLine($"-- ID: {bankacc.BankAccountId}");
                    sb.AppendLine($"--- Balance: {bankacc.Balance:f2}");
                    sb.AppendLine($"--- Bank: {bankacc.BankName}");
                    sb.AppendLine($"--- SWIFT: {bankacc.SWIFTCode}");
                }

                sb.AppendLine($"Credit Cards: {acc.creditCardAacc.Count}");

                foreach (var cc in acc.creditCardAacc)
                {
                    sb.AppendLine($"-- ID: {cc.CreditCardId}");
                    sb.AppendLine($"--- Limit: {cc.Limit:f2}");
                    sb.AppendLine($"--- Money Owed: {cc.MoneyOwed:f2}");
                    sb.AppendLine($"--- Limit Left: {cc.LimitLeft:f2}");
                    sb.AppendLine($"--- Expiration Date: {cc.ExpirationDate}");
                }
            }

            return sb.ToString().TrimEnd() ;
        }
    }
}
