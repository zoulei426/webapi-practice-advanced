using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YuLinTu.Practice.Books
{
    public interface IBookAppService :
          ICrudAppService<
              BookDto,
              Guid,
              PagedAndSortedResultRequestDto,
              CreateUpdateBookDto>
    {
        Task<BookDto> GetBookForAuthorAsync(Guid authorId, Guid bookId);

        Task<BookDto> CreateBookForAuthorAsync(Guid authorId, CreateUpdateBookDto book);

        Task UpdateBookForAuthorAsync(Guid authorId, Guid bookId, CreateUpdateBookDto book);
    }
}