using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    public class PracticeHttpApiHostMigrationsDbContext : AbpDbContext<PracticeHttpApiHostMigrationsDbContext>
    {
        public PracticeHttpApiHostMigrationsDbContext(DbContextOptions<PracticeHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePractice();
        }
    }
}
