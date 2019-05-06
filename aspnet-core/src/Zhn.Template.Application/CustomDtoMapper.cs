using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Zhn.Template.Authorization.Users;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<UserEditDto, User>();
        }
    }
}