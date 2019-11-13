using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Snow.Template.Authorization
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

            var mentItems =
                administration.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems, L("MenuItems"), multiTenancySides: MultiTenancySides.Host);
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Create, L("CreatingNewMenuItem"), multiTenancySides: MultiTenancySides.Host);
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Edit, L("EditingMenuItem"), multiTenancySides: MultiTenancySides.Host);
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_Delete, L("DeletingMenuItem"), multiTenancySides: MultiTenancySides.Host);
            mentItems.CreateChildPermission(PermissionNames.Pages_Administration_MenuItems_BatchDelete, L("BatchDeletingMenuItem"), multiTenancySides: MultiTenancySides.Host);

            var languages = administration.CreateChildPermission(PermissionNames.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            var parameterTypes = administration.CreateChildPermission(PermissionNames.Pages_Administration_ParameterTypes, L("ParameterTypes"));
            parameterTypes.CreateChildPermission(PermissionNames.Pages_Administration_ParameterTypes_Create, L("CreatingNewParameterType"));
            parameterTypes.CreateChildPermission(PermissionNames.Pages_Administration_ParameterTypes_Edit, L("EditingParameterType"));
            parameterTypes.CreateChildPermission(PermissionNames.Pages_Administration_ParameterTypes_Delete, L("DeletingParameterType"));
            parameterTypes.CreateChildPermission(PermissionNames.Pages_Administration_ParameterTypes_BatchDelete, L("BatchDeletingParameterType"));

            var parameters = administration.CreateChildPermission(PermissionNames.Pages_Administration_Parameters, L("Parameters"));
            parameters.CreateChildPermission(PermissionNames.Pages_Administration_Parameters_Create, L("CreatingNewParameter"));
            parameters.CreateChildPermission(PermissionNames.Pages_Administration_Parameters_Edit, L("EditingParameter"));
            parameters.CreateChildPermission(PermissionNames.Pages_Administration_Parameters_Delete, L("DeletingParameter"));
            parameters.CreateChildPermission(PermissionNames.Pages_Administration_Parameters_BatchDelete, L("BatchDeletingParameter"));

            var tenants = administration.CreateChildPermission(PermissionNames.Pages_Administration_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Administration_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Administration_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Administration_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Administration_Tenants_BatchDelete, L("BatchDeletingTenant"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}