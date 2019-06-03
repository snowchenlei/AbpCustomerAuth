using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Zhn.Template.Configuration;
using Zhn.Template.Web;

namespace Zhn.Template.EntityFrameworkCore
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
