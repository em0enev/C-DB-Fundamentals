using BillsPaymentSystem.Data.EntityConfiguration;
using BillsPaymentSystem.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data
{
    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext(DbContextOptions options) : base(options)
        {
        }

        public BillsPaymentSystemContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Config = @"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=BillsPaymentDB;Integrated Security = true;";

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new CreditCardConfig());
            modelBuilder.ApplyConfiguration(new BankAccountConfig());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfig());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }
    }
}
