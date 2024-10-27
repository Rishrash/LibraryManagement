using AutoMapper;
using LibraryManagement.API.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateBookDTO, Book>();

            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();

            CreateMap<BorrowingRecord, BorrowingRecordDTO>();
            CreateMap<CreateBorrowingRecordDTO, BorrowingRecord>();
        }
    }
}