using Abp.Authorization;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Users;

namespace Snow.Template.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}



