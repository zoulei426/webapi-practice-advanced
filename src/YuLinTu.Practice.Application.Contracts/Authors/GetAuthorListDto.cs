using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Authors
{
    public class GetAuthorListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}