using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.EntityFrameworkCore
{
    public class TemplateSecondDbContext : AbpDbContext
    {
        public DbSet<Person> Person { get; set; }
        public TemplateSecondDbContext(DbContextOptions<TemplateSecondDbContext> options)
            : base(options)
        {

        }
    }

    
}
