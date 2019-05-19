﻿using Abp.MultiTenancy;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.Authorization.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}


