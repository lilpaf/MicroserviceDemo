using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext dbContext, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");

                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!dbContext.Platforms.Any()) 
            {
                Console.WriteLine("--> Seeding Data...");

                dbContext.AddRange(
                    new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost="Free"},
                    new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost="Free"},
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost="Free"}
                );

                dbContext.SaveChanges();

                return;
            }

            Console.WriteLine("--> We already have data");
        }
    }
}
