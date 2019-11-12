using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    /// <summary>
    /// 租户列表数据
    /// </summary>
    public class TenantListDto : EntityDto
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}