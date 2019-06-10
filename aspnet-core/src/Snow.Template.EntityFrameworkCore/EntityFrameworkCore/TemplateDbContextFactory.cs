using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Snow.Template.Configuration;
using Snow.Template.Web;

namespace Snow.Template.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class TemplateDbContextFactory : DbContextFactory<TemplateDbContext>//IDesignTimeDbContextFactory<TemplateDbContext>
    {
        public override string ConnectionStringName => TemplateConsts.ConnectionStringName;

        public override TemplateDbContext CreateDbContext(DbContextOptions<TemplateDbContext> options)
        {
            return new TemplateDbContext(options);
        }

        //public TemplateDbContext CreateDbContext(string[] args)
        //{
        //    var builder = new DbContextOptionsBuilder<TemplateDbContext>();
        //    var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

        //    TemplateDbContextConfigurer.Configure(builder, configuration.GetConnectionString(TemplateConsts.ConnectionStringName));

        //    return new TemplateDbContext(builder.Options);
        //}
    }
}



