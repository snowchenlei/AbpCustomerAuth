using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.Template.Authorization.MultiTenancy.Dto;

namespace Snow.Template.Web.Models.Tenants
{
    /// <summary>
    /// 修改租户ViewModel
    /// </summary>
    public class EditTenantViewModel
    {
        /// <summary>
        /// 修改租户模型
        /// </summary>
        public TenantEditDto Tenant { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tenant"></param>
        public EditTenantViewModel(TenantEditDto tenant)
        {
            Tenant = tenant;
        }
    }
}