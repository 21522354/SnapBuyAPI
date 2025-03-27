using AutoMapper;
using UserService.Models.Dtos.RequestModels;
using UserService.Models.Dtos.ResponseModels;
using UserService.Models.Entities;

namespace UserService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, MRes_User>().ReverseMap();
            CreateMap<User, MReq_UserNameImage>();
            CreateMap<User, MReq_User>().ReverseMap();
        }
    }
}
