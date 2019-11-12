using Abp.AspNetCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Snow.Template.Configuration;
using Snow.Template.Interceptor;

namespace Snow.Template.Web.Startup
{
    [DependsOn(typeof(TemplateWebCoreModule))]
    public class TemplateWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TemplateWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //Configuration.Navigation.Providers.Add<TemplateNavigationProvider>();
            Configuration.Modules.AbpAspNetCore().UseMvcDateTimeFormatForAppServices = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TemplateWebMvcModule).GetAssembly());
        }
    }
}