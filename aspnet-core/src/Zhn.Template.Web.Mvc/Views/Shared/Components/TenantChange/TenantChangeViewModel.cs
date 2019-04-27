using Abp.AutoMapper;
using Zhn.Template.Sessions.Dto;

namespace Zhn.Template.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}


