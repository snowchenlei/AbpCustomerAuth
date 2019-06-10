using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Users;
using Snow.Template.Configuration;
using Snow.Template.Interceptor;
using Snow.Template.Localization;
using Snow.Template.Authorization.MultiTenancy;
using Snow.Template.Timing;

namespace Snow.Template
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class TemplateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //DataAnnotations本地化
            ValidationInterceptorRegister.Initialize(IocManager);

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            TemplateLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = TemplateConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TemplateCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
