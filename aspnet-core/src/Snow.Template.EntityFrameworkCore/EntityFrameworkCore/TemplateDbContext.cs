using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Users;
using Snow.Template.Authorization.MultiTenancy;
using Microsoft.Extensions.Logging.Debug;
using Snow.Template.Parameters;

namespace Snow.Template.EntityFrameworkCore
{
    public class TemplateDbContext : AbpZeroDbContext<Tenant, Role, User, TemplateDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<MenuItem> MenuItem { get; set; }

        public DbSet<ParameterType> ParameterType { get; set; }
        public DbSet<Parameter> Parameter { get; set; }

        //输出到debug输出
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });

        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
    }
}