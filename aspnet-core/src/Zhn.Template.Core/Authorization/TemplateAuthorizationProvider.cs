using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Zhn.Template.Authorization
{
    public class TemplateAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                        context.CreatePermission(PermissionNames.Pages, L("Pages"));
            var administration = pages.CreateChildPermission(PermissionNames.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(PermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_BatchDelete, L("BatchDeletingRole"));

            var users = administration.CreateChildPermission(PermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_BatchDelete, L("BatchDeletingUser"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_Tenants, L("Tenants"),
                multiTenancySides: MultiTenancySides.Host);

            var mentItems =
                administration.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems, L("MenuItems"));
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Create, L("CreatingNewMenuItem"));
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Edit, L("EditingMenuItem"));
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Delete, L("DeletingMenuItem"));
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_BatchDelete, L("BatchDeletingMenuItem"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}