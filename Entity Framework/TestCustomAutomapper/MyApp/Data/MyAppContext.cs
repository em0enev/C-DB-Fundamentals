namespace MyApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions options) : base(options)
        {

        }

        public MyAppContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=MyAppDb;Integrated Security = true;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Employee> Employees { get; set; }


    }
}
