using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Snow.Template.Configuration;
using Snow.Template.Web;

namespace Snow.Template.EntityFrameworkCore
{
    public class TemplateSecondDbContextFactory : DbContextFactory<TemplateSecondDbContext>
    {
        public override string ConnectionStringName => TemplateConsts.SecondDbConnectionStringName;
        public override TemplateSecondDbContext CreateDbContext(DbContextOptions<TemplateSecondDbContext> options)
        {
            return new TemplateSecondDbContext(options);
        }
    }
}

