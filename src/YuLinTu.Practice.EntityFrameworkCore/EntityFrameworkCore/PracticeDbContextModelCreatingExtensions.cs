using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YuLinTu.Practice.Authors;
using YuLinTu.Practice.Books;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    public static class PracticeDbContextModelCreatingExtensions
    {
        public static void ConfigurePractice(
            this ModelBuilder builder,
            Action<PracticeModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PracticeModelBuilderConfigurationOptions(
                PracticeDbProperties.DbTablePrefix,
                PracticeDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Book>(b =>
            {
                b.ToTable(options.TablePrefix + "Books",
                          options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });

            builder.Entity<Author>(b =>
            {
                b.ToTable(options.TablePrefix + "Authors",
                    options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.HasIndex("FirstName", "LastName");
            });

            // 将数据库字段全小写且通过下划线分隔
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }
        }
    }
}