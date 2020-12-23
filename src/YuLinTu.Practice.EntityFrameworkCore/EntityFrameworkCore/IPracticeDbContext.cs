using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using YuLinTu.Practice.Authors;
using YuLinTu.Practice.Books;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    [ConnectionStringName(PracticeDbProperties.ConnectionStringName)]
    public interface IPracticeDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Book> Books { get; }

        DbSet<Author> Authors { get; }
    }
}