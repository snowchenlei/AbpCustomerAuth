using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Zhn.Template.Authorization.MenuItems;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Users;
using Zhn.Template.MultiTenancy;

namespace Zhn.Template.EntityFrameworkCore
{
    public class TemplateDbContext : AbpZeroDbContext<Tenant, Role, User, TemplateDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<MenuItem> MenuItem { get; set; }
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
        {
        }
    }
}


