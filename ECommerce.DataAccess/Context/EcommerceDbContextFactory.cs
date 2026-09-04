using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.DataAccess.Context
{
    public class EcommerceDbContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
    {
        public EcommerceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();

            // Use an environment variable for design-time or fallback to LocalDB
            var conn = Environment.GetEnvironmentVariable("E_COMMERCE_CONNECTION")
                       ?? "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;";

            // Allow overriding migrations assembly via env var for flexibility in design-time tools
            var migrationsAssembly = Environment.GetEnvironmentVariable("MIGRATIONS_ASSEMBLY")
                                    ?? "ECommerce.DataAccess";

            optionsBuilder.UseSqlServer(conn, b => b.MigrationsAssembly(migrationsAssembly));

            return new EcommerceDbContext(optionsBuilder.Options);
        }
    }
}
