﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Zhn.Template.Dto;

namespace Zhn.Template.Auditing.Dto
{
    public class GetAuditLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public GetAuditLogsInput()
        {
            StartDate = DateTime.Now.AddDays(-10);
            EndDate = DateTime.Now;
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ExecutionTime DESC";
            }

            if (Sorting.IndexOf("UserName", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Sorting = "User." + Sorting;
            }
            else
            {
                Sorting = "AuditLog." + Sorting;
            }
        }
    }
}
