using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Authors
{
    public class AuthorAppService : PracticeAppService, IAuthorAppService
    {
        private readonly IAuthorRepository authorRepository;
        private readonly AuthorManager authorManager;

        public AuthorAppService(IAuthorRepository authorRepository, AuthorManager authorManager)
        {
            this.authorRepository = authorRepository;
            this.authorManager = authorManager;
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
        {
            var author = await authorManager.CreateAsync(
                input.FirstName,
                input.LastName,
                input.BirthDate,
                input.ShortBio);

            await authorRepository.InsertAsync(author);

            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task DeleteAsync(Guid id)
        {
            await authorRepository.DeleteAsync(id);
        }

        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Author.FirstName);
            }

            var authors = await authorRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter
            );

            var totalCount = await AsyncExecuter.CountAsync(
                authorRepository.WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    author => author.FirstName.Contains(input.Filter)
                    || author.LastName.Contains(input.Filter)
                )
            );

            return new PagedResultDto<AuthorDto>(
                totalCount,
                ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
            );
        }

        public async Task UpdateAsync(Guid id, UpdateAuthorDto input)
        {
            var author = await authorRepository.GetAsync(id);

            if (author.FirstName != input.FirstName || author.LastName != input.LastName)
            {
                await authorManager.ChangeNameAsync(author, input.FirstName, input.LastName);
            }

            author.BirthDate = input.BirthDate;
            author.ShortBio = input.ShortBio;

            await authorRepository.UpdateAsync(author);
        }
    }
}