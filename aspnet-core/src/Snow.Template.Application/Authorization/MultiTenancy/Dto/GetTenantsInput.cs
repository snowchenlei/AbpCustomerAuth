using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    /// <summary>
    /// 分页查询租户
    /// </summary>
    public class GetTenantsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }

        public void Normalize()
        {
            if (String.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}