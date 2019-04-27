using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Zhn.Template.Configuration;
using Zhn.Template.EntityFrameworkCore;
using Zhn.Template.Migrator.DependencyInjection;

namespace Zhn.Template.Migrator
{
    [DependsOn(typeof(TemplateEntityFrameworkModule))]
    public class TemplateMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public TemplateMigratorModule(TemplateEntityFrameworkModule abpTemplateEntityFrameworkModule)
        {
            abpTemplateEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(TemplateMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                TemplateConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TemplateMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}



