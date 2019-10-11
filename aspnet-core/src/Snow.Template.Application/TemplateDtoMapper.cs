using System;
using System.Collections.Generic;
using System.Text;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using AutoMapper;
using Snow.Template.Auditing.Dto;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.MenuItems.Dto;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Roles.Dto;
using Snow.Template.Authorization.Users;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Parameters;
using Snow.Template.Parameters.Dto;

namespace Snow.Template
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
            //MenuItem
            configuration.CreateMap<MenuItem, MenuItemListDto>()
                .ForMember(entity => entity.ParentName,
                    opt => opt.MapFrom(src => (src.Parent != null ? src.Parent.Name : String.Empty)));
            configuration.CreateMap<MenuItem, MenuItemSelectListDto>();
            configuration.CreateMap<MenuItemEditDto, MenuItem>();
            configuration.CreateMap<MenuItem, MenuItemCacheItem>()
                .ForMember(entity => entity.ParentId,
                    opt => opt.MapFrom(src => (src.Parent != null ? src.Parent.Id : 0)));
            configuration.CreateMap<MenuItemCacheItem, MenuItemEditDto>();
            //configuration.CreateMap<MenuItem, MenuItemEditDto>()
            //    .ForMember(entity => entity.ParentId,
            //        opt => opt.MapFrom(src => (src.Parent != null ? src.Parent.Id : 0)));
            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            //configuration.CreateMap<EntityChange, EntityChangeListDto>();
            //Parameter
            configuration.CreateMap<Parameter, ParameterListDto>()
                .ForMember(entity => entity.TypeName,
                    opt => opt.MapFrom(src => (src.ParameterType != null ? src.ParameterType.Name : String.Empty)));
            configuration.CreateMap<ParameterType, ParameterTypeSelectListDto>();
            configuration.CreateMap<ParameterEditDto, Parameter>();
            configuration.CreateMap<Parameter, ParameterEditDto>();
            configuration.CreateMap<ParameterType, ParameterTypeDto>()
                .ForMember(entity => entity.Text,
                    opt => opt.MapFrom(src => (src.Name)));
        }
    }
}