using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization;
using Abp.Authorization.Users;
using AutoMapper;
using Zhn.Template.Authorization.MenuItems;
using Zhn.Template.Authorization.MenuItems.Dto;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Authorization.Users;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template
{
    internal static class TemplateDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //User
            configuration.CreateMap<UserEditDto, User>();
            configuration.CreateMap<User, UserListDto>().ForMember(x => x.Roles, opt => opt.Ignore());
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            //UserRole
            configuration.CreateMap<UserRole, UserListRoleDto>();
            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>()
                .ForMember(entity => entity.ParentName,
                    opt => opt.MapFrom(src => (src.Parent != null ? src.Parent.Name : null)));
            //Permission
            configuration.CreateMap<MenuItem, MenuItemListDto>();
        }
    }
}