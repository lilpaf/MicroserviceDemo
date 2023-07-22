using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static async Task PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = await grpcClient.ReturnAllPlatformsAsync();

                var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();

                await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd, repo, platforms);
            }
        }

        private static async Task SeedData(
            AppDbContext dbContext, 
            bool isProd, 
            ICommandRepo repo, 
            IEnumerable<Platform> platforms)
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

            if (repo is not null) 
            {
                Console.WriteLine("--> Seeding new platforms...");

                foreach (var plat in platforms)
                {
                    if (!await repo.ExternalPlatformExistsAsync(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                    }
                    await repo.SaveChangesAsync();
                }
            }
        }
    }
}
