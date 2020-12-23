using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using YuLinTu.Practice.Authors;

namespace YuLinTu.Practice.Books
{
    public class BookAppService :
         CrudAppService<
             Book,
             BookDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdateBookDto>,
         IBookAppService
    {
        private readonly IAuthorRepository authorRepository;

        public BookAppService(IRepository<Book, Guid> repository, IAuthorRepository authorRepository)
            : base(repository)
        {
            this.authorRepository = authorRepository;
        }

        public async override Task<BookDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var query = from book in Repository
                        join author in authorRepository on book.AuthorId equals author.Id
                        where book.Id == id
                        select new { book, author };

            // 获取 book 和 author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.FirstName + queryResult.author.LastName;
            return bookDto;
        }

        public async override Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await CheckGetListPolicyAsync();

            var query = from book in Repository
                        join author in authorRepository on book.AuthorId equals author.Id
                        orderby input.Sorting
                        select new { book, author };

            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var bookDtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.AuthorName = x.author.FirstName + x.author.LastName;
                return bookDto;
            }).ToList();

            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<BookDto>(
                totalCount,
                bookDtos
            );
        }
    }
}