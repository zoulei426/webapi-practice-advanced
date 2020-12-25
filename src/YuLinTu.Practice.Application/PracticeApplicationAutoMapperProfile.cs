using AutoMapper;
using System;
using System.Linq;
using YuLinTu.Practice.Authors;
using YuLinTu.Practice.Books;

namespace YuLinTu.Practice
{
    public class PracticeApplicationAutoMapperProfile : Profile
    {
        public PracticeApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>();
            CreateMap<BookDto, CreateUpdateBookDto>();

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => (DateTime.Now - src.BirthDate).Days / 365));

            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();

            ForAllMaps((typeMap, mappingExpr) =>
            {
                // 忽略未映射的映射
                foreach (var dest in typeMap.DestinationTypeDetails.PublicReadAccessors)
                {
                    if (!typeMap.PropertyMaps.Any(t => t.DestinationName.Equals(dest.Name)))
                    {
                        mappingExpr.ForMember(dest.Name, opt => opt.Ignore());
                    }
                }
            });
        }
    }
}