using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Zhn.Template.Configuration;
using Zhn.Template.Web;

namespace Zhn.Template.EntityFrameworkCore
{
    /// <summary>
    /// EF Core PMC commands 基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbContextFactory<T>: IDesignTimeDbContextFactory<T> where T:AbpDbContext
    {
        /// <summary>
        /// 要采用的数据库连接节点名称
        /// </summary>
        public abstract string ConnectionStringName { get; }
        /// <summary>
        /// 创建DbContext实例
        /// 如果觉得每个数据库都要自己new太low，那么可以采用反射来动态创建，毕竟这里也只是PMC command使用的
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public abstract T CreateDbContext(DbContextOptions<T> options);

        public T CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<T>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            TemplateDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ConnectionStringName));

            return this.CreateDbContext(builder.Options);
        }
    }
}
