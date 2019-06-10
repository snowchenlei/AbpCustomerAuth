using Abp.Domain.Uow;
using Abp.Configuration.Startup;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Snow.Template.EntityFrameworkCore.Seed;

namespace Snow.Template.EntityFrameworkCore
{
    [DependsOn(
        typeof(TemplateCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class TemplateEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                //Configuration.ReplaceService<IConnectionStringResolver, MyConnectionStringResolver>();

                // Configure first DbContext
                Configuration.Modules.AbpEfCore().AddDbContext<TemplateDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        TemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        TemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
                // Configure second DbContext
                Configuration.Modules.AbpEfCore().AddDbContext<TemplateSecondDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        TemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        TemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TemplateEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
