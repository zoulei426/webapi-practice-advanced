using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using YuLinTu.Practice.EntityFrameworkCore;

namespace YuLinTu.Practice.Authors
{
    public class EfCoreAuthorRepository
           : EfCoreRepository<PracticeDbContext, Author, Guid>,
               IAuthorRepository
    {
        public EfCoreAuthorRepository(
            IDbContextProvider<PracticeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Author> FindByNameAsync(string firstName, string lastName)
        {
            return await DbSet.FirstOrDefaultAsync(
                author => author.FirstName == firstName
                       && author.LastName == lastName);
        }

        public async Task<List<Author>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null)
        {
            return await DbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    author => author.FirstName.Contains(filter)
                    || author.LastName.Contains(filter)
                 )
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}