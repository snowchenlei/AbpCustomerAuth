using AutoMapper;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template.Authorization.Users.Mapper
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {

            CreateMap<UserEditDto, User>();
            CreateMap<User,UserListDto>().ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}


