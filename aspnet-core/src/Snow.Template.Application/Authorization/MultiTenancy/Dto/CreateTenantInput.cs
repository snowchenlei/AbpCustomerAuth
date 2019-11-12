using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Snow.Template.Localization;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    public class CreateTenantInput
    {
        [Required]
        [TemplateDisplayName("TenancyName")]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [TemplateDisplayName("Name")]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [TemplateDisplayName("AdminEmailAddress")]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [TemplateDisplayName("ConnectionString")]
        [StringLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }
    }
}