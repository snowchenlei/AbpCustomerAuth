using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization.Users;

namespace Snow.Template.EntityFrameworkCore
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

