using Abp.Runtime.Validation;
using Snow.Template.Dto;

namespace Snow.Template.Authorization.Roles.Dto
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
