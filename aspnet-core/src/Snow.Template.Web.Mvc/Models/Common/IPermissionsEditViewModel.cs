using System.Collections.Generic;
using Snow.Template.Authorization.Roles.Dto;

namespace Snow.Template.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
        List<string> GrantedPermissionNames { get; set; }
    }
}
