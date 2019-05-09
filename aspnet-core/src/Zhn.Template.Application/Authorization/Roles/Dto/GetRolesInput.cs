using Abp.Runtime.Validation;
using Zhn.Template.Dto;

namespace Zhn.Template.Authorization.Roles.Dto
{
    public class GetRolesInput : PagedAndSortedInputDto, IShouldNormalize, IGetRolesInput

    {
        public string Permission { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}