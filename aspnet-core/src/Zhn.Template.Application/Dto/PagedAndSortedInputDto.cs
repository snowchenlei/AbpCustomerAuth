using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Zhn.Template.Dto
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}
