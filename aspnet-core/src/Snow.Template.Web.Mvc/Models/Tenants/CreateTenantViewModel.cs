using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.Template.Authorization.MultiTenancy.Dto;

namespace Snow.Template.Web.Models.Tenants
{
    public class CreateTenantViewModel
    {
        public CreateTenantInput Tenant { get; set; }

        public CreateTenantViewModel()
        {
            Tenant = new CreateTenantInput();
        }
    }
}