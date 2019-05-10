using System.Collections.Generic;
using Zhn.Template.Authorization.Roles.Dto;

namespace Zhn.Template.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
        List<string> GrantedPermissionNames { get; set; }
    }
}