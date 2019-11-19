using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Snow.Template.Authentication.JwtBearer;
using Snow.Template.Configuration;
using Snow.Template.Identity;
using Snow.Template.Web.Resources;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Json;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using Snow.Template.Web.Validation;
using Abp.AspNetCore.Mvc.Antiforgery;
using Newtonsoft.Json.Serialization;
using Abp.Dependency;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Snow.Template.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            })
            .AddFluentValidation(fv =>
            {
                //禁用其它的认证
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();
            services.AddSignalR();

            // Configure Abp and Dependency Injection
            return services.AddAbp<TemplateWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); // Initializes ABP framework.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "AppData")),
                RequestPath = "/AppData"
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}