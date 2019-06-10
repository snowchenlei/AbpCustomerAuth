using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.Roles.Dto
{
    public class CreateOrUpdateRoleInput
    {
        public RoleEditDto Role { get; set; }
        public List<string> Permissions { get; set; }
    }
}
