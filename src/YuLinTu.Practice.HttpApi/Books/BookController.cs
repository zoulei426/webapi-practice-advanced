using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using YuLinTu.Practice.Authors;

namespace YuLinTu.Practice.Books
{
    [RemoteService]
    [Route("api/practice/authors/{authorId}/books")]
    public class BookController : PracticeController
    {
        private readonly IBookAppService bookAppService;
        private readonly IAuthorAppService authorAppService;

        public BookController(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            Check.NotNull(bookAppService, nameof(bookAppService));
            Check.NotNull(authorAppService, nameof(authorAppService));

            this.bookAppService = bookAppService;
            this.authorAppService = authorAppService;
        }

        /// <summary>
        /// 获取作者的书籍
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (!await authorAppService.IsExistedAsync(authorId))
                return NotFound();

            var result = await bookAppService.GetBookForAuthorAsync(authorId, bookId);
            return Ok(result);
        }

        /// <summary>
        /// 为作者创建书籍
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBookForAuthor(Guid authorId, CreateUpdateBookDto book)
        {
            if (!await authorAppService.IsExistedAsync(authorId))
                return NotFound();

            var result = await bookAppService.CreateBookForAuthorAsync(authorId, book);
            return Ok(result);
        }

        /// <summary>
        /// 为作者更新书籍
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBookForAuthor(Guid authorId, Guid bookId, CreateUpdateBookDto book)
        {
            if (!await authorAppService.IsExistedAsync(authorId))
                return NotFound();

            await bookAppService.UpdateBookForAuthorAsync(authorId, bookId, book);

            return NoContent();
        }

        /// <summary>
        /// 为作者局部更新书籍
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPatch("{bookId}")]
        public async Task<IActionResult> PartiallyUpdateBookForAuthor(
            Guid authorId,
            Guid bookId,
            JsonPatchDocument<CreateUpdateBookDto> patchDocument)
        {
            if (!await authorAppService.IsExistedAsync(authorId))
                return NotFound();

            var bookDto = await bookAppService.GetBookForAuthorAsync(authorId, bookId);

            var dtoToPatch = ObjectMapper.Map<BookDto, CreateUpdateBookDto>(bookDto);

            patchDocument.ApplyTo(dtoToPatch, ModelState);

            await bookAppService.UpdateBookForAuthorAsync(authorId, bookId, dtoToPatch);

            return NoContent();
        }
    }
}