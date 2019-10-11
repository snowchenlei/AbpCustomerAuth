using AutoMapper;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Snow.Template.Authorization.Roles;
using System.Linq;

namespace Snow.Template.Authorization.Roles.Dto
{
    public class RoleMapProfile : Profile
    {
        public RoleMapProfile()
        {
            // Role and permission
            CreateMap<Permission, string>().ConvertUsing(r => r.Name);
            CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

            CreateMap<RoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());

            CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
               opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));

            CreateMap<Role, RoleListDto>();
            CreateMap<Role, RoleEditDto>();
            CreateMap<Permission, FlatPermissionDto>();
        }
    }
}