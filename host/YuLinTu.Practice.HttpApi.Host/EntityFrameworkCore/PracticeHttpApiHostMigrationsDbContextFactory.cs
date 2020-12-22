using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    public class PracticeHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<PracticeHttpApiHostMigrationsDbContext>
    {
        public PracticeHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PracticeHttpApiHostMigrationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Practice"));

            return new PracticeHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}