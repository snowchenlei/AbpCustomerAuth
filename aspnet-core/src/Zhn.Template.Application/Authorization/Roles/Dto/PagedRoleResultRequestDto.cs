using Abp.Application.Services.Dto;

namespace Zhn.Template.Authorization.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}



