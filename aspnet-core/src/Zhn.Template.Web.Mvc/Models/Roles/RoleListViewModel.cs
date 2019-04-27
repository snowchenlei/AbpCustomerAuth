using System.Collections.Generic;
using Zhn.Template.Authorization.Roles.Dto;

namespace Zhn.Template.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<RoleListDto> Roles { get; set; }

        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}


