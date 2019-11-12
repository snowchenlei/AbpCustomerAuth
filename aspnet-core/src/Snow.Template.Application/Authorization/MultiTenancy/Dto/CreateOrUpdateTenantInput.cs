using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    public class CreateOrUpdateTenantInput
    {
        public TenantEditDto Tenant { get; set; }
    }
}