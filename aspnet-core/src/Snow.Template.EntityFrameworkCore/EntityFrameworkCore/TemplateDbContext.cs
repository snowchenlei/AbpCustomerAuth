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
using Snow.Template.Storage;

namespace Snow.Template.EntityFrameworkCore
{
    public class TemplateDbContext : AbpZeroDbContext<Tenant, Role, User, TemplateDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<MenuItem> MenuItem { get; set; }

        public virtual DbSet<ParameterType> ParameterType { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }


        //输出到debug输出
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // TODO:记录EfCore日志
            //optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
    }
}