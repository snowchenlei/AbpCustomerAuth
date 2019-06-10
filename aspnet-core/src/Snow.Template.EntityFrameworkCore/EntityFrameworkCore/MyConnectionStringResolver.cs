using System;
using System.Collections.Generic;
using System.Text;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using Snow.Template.Configuration;
using Snow.Template.Web;

namespace Snow.Template.EntityFrameworkCore
{
    public class MyConnectionStringResolver : DefaultConnectionStringResolver
    {
        public MyConnectionStringResolver(IAbpStartupConfiguration configuration)
            : base(configuration)
        {
        }

        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args["DbContextConcreteType"] as Type == typeof(TemplateSecondDbContext))
            {
                var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
                return configuration.GetConnectionString(TemplateConsts.SecondDbConnectionStringName);
            }

            return base.GetNameOrConnectionString(args);
        }
    }
}

