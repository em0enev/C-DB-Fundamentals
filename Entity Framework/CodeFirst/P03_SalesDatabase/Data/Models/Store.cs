using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        public Store()
        {
            this.Sales = new List<Sale>();
            this.Products = new List<Product>();
            this.Customers = new List<Customer>();
        }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Customer> Customers { get; set; }

    }
}
