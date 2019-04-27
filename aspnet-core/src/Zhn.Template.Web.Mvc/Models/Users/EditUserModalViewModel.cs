using System.Collections.Generic;
using System.Linq;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}


