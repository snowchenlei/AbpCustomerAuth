using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Snow.Template.Configuration;
using Snow.Template.EntityFrameworkCore;
using Snow.Template.Migrator.DependencyInjection;

namespace Snow.Template.Migrator
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




