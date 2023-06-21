using AutoMapper;
using TestApi.Models.DTO;

namespace TestApi.Models.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() {
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
        }
    }
}
