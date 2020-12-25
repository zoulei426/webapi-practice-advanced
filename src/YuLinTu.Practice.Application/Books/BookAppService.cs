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

        public async Task<BookDto> CreateBookForAuthorAsync(Guid authorId, CreateUpdateBookDto book)
        {
            var author = await authorRepository.GetAsync(author => author.Id == authorId);
            if (author is null)
            {
                throw new EntityNotFoundException(typeof(Author), authorId);
            }

            var entity = ObjectMapper.Map<CreateUpdateBookDto, Book>(book);
            entity.AuthorId = authorId;
            var result = await Repository.InsertAsync(entity);
            var bookDto = ObjectMapper.Map<Book, BookDto>(result);
            bookDto.AuthorName = author.GetFullName();
            return bookDto;
        }

        public async Task<BookDto> GetBookForAuthorAsync(Guid authorId, Guid bookId)
        {
            var author = await authorRepository.GetAsync(author => author.Id == authorId);
            if (author is null)
            {
                throw new EntityNotFoundException(typeof(Author), authorId);
            }
            var book = await Repository.GetAsync(book => book.Id == bookId && book.AuthorId == authorId);
            if (book is null)
            {
                throw new EntityNotFoundException(typeof(Book), bookId);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(book);
            bookDto.AuthorName = author.GetFullName();
            return bookDto;
        }

        public async Task UpdateBookForAuthorAsync(Guid authorId, Guid bookId, CreateUpdateBookDto book)
        {
            var entity = await Repository.GetAsync(book => book.Id == bookId && book.AuthorId == authorId);
            if (entity is null)
            {
                throw new EntityNotFoundException(typeof(Book), bookId);
            }

            ObjectMapper.Map(book, entity);

            await Repository.UpdateAsync(entity);
        }
    }
}