using Abp.Authorization;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}


