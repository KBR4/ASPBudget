using Application.Dtos;
using Domain.Entities;
using AutoMapper;
using Application.Requests;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Budget, BudgetDto>().ReverseMap();
            CreateMap<BudgetResult, BudgetResultDto>().ReverseMap();
            CreateMap<BudgetRecord, BudgetRecordDto>().ReverseMap();

            CreateMap<UpdateUserRequest, User>();
            CreateMap<RegistrationRequest, User>();
        }    
    }
}
