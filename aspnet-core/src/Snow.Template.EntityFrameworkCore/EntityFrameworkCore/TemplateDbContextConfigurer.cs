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
            dbContextOptions.UseMySql(connectionString);
        }

        public static void Configure<T>(DbContextOptionsBuilder<T> builder, DbConnection connection)
            where T : AbpDbContext
        {
            builder.UseMySql(connection);
        }

        public static void Configure(DbContextOptionsBuilder<TemplateDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TemplateDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}