using System.Collections.Generic;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}


