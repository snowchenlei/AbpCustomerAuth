using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.ParameterManager.ParameterTypes.Dto;

namespace Snow.Template.ParameterManager.ParameterTypes
{
    public interface IParameterTypeAppService : IApplicationService
    {
        Task<PagedResultDto<ParameterTypeListDto>> GetPaged(GetParameterTypesInput input);

        Task<GetParameterTypeForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        Task CreateOrEdit(CreateOrUpdateParameterTypeInput input);

        Task Delete(EntityDto<Guid> input);
    }
}