using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApi.Models;

namespace StoreApi.Data
{
    public sealed class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<SalesLine> SalesLine { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Sinpe> Sinpe { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}