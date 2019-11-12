using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Snow.Template.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Snow.Template.Web.Host.Startup
{
    [DependsOn(
       typeof(TemplateWebCoreModule))]
    public class TemplateWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TemplateWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TemplateWebHostModule).GetAssembly());
        }
    }
}