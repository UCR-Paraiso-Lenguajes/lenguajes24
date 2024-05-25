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

        public void EnsureDatabaseCreated()
        {
            Database.ExecuteSqlRaw("CREATE DATABASE IF NOT EXISTS sys;");
            Database.ExecuteSqlRaw("USE sys;");
            var tableCreationScripts = new string[]
            {
                @"
                CREATE TABLE IF NOT EXISTS `Category` (
                  `uuid` varchar(36) NOT NULL,
                  `name` varchar(100) NOT NULL,
                  PRIMARY KEY (`uuid`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                ",
                @"
                CREATE TABLE IF NOT EXISTS `Product` (
                  `name` varchar(50) NOT NULL,
                  `uuid` varchar(36) NOT NULL,
                  `imageUrl` varchar(100) NOT NULL,
                  `price` decimal(10,0) NOT NULL,
                  `description` text NOT NULL,
                  `category` varchar(100) NOT NULL,
                  PRIMARY KEY (`uuid`),
                  KEY `Product_Category_FK` (`category`),
                  CONSTRAINT `Product_Category_FK` FOREIGN KEY (`category`) REFERENCES `Category` (`uuid`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                ",
                @"
                CREATE TABLE IF NOT EXISTS `Sales` (
                  `uuid` varchar(36) NOT NULL,
                  `date` date DEFAULT NULL,
                  `confirmation` tinyint(1) DEFAULT NULL,
                  `paymentMethod` varchar(36) DEFAULT NULL,
                  `total` decimal(10,0) DEFAULT NULL,
                  `address` text,
                  `PurchaseNumber` varchar(20) NOT NULL,
                  PRIMARY KEY (`uuid`),
                  KEY `Sales_PaymentMethods_FK` (`paymentMethod`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                ",
                @"
                CREATE TABLE IF NOT EXISTS `SalesLine` (
                  `uuid` varchar(36) NOT NULL,
                  `quantity` int DEFAULT NULL,
                  `subtotal` decimal(10,0) DEFAULT NULL,
                  `uuidProduct` varchar(36) DEFAULT NULL,
                  `uuidSales` varchar(36) DEFAULT NULL,
                  PRIMARY KEY (`uuid`),
                  KEY `SL_P_FK` (`uuidProduct`),
                  KEY `SL_S_FK` (`uuidSales`),
                  CONSTRAINT `SalesLine_Product_FK` FOREIGN KEY (`uuidProduct`) REFERENCES `Product` (`uuid`),
                  CONSTRAINT `SalesLine_Sales_FK` FOREIGN KEY (`uuidSales`) REFERENCES `Sales` (`uuid`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                ",
                @"
                CREATE TABLE IF NOT EXISTS `Sinpe` (
                  `uuid` varchar(36) NOT NULL,
                  `uuidSales` varchar(36) DEFAULT NULL,
                  `confirmationNumber` varchar(36) DEFAULT NULL,
                  PRIMARY KEY (`uuid`),
                  KEY `Sinpe_Sales_FK` (`uuidSales`),
                  CONSTRAINT `Sinpe_Sales_FK` FOREIGN KEY (`uuidSales`) REFERENCES `Sales` (`uuid`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                "
            };

            foreach (var script in tableCreationScripts)
            {
                Database.ExecuteSqlRaw(script);
            }
        }
    }
}