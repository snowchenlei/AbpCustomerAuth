using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public UserEditDto User { get; set; }
        public UserRoleDto[] Roles { get; set; }
    }
}
