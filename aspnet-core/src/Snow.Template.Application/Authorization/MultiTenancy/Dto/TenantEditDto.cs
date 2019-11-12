using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Snow.Template.Localization;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    /// <summary>
    /// 租户编辑
    /// </summary>
    public class TenantEditDto : NullableIdDto
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

        public bool IsActive { get; set; }
    }
}