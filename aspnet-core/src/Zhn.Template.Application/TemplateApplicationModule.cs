using Abp.AutoMapper;
using Abp.FluentValidation;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Zhn.Template.Authorization;

namespace Zhn.Template
{
    [DependsOn(
        typeof(TemplateCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule))]
    public class TemplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<TemplateAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(TemplateApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}