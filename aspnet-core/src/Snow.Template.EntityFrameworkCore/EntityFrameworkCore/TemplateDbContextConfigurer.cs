using System.Data.Common;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Snow.Template.EntityFrameworkCore
{
    public static class TemplateDbContextConfigurer
    {

        public static void Configure<T>(DbContextOptionsBuilder<T> dbContextOptions, string connectionString)
            where T : AbpDbContext
        {
            dbContextOptions.UseSqlServer(connectionString);
        }

        public static void Configure<T>(DbContextOptionsBuilder<T> builder, DbConnection connection)
            where T : AbpDbContext
        {
            builder.UseSqlServer(connection);
        }

        public static void Configure(DbContextOptionsBuilder<TemplateDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TemplateDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}



