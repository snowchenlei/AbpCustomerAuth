using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.ParameterManager.ParameterTypes.Dto;

namespace Snow.Template.ParameterManager.ParameterTypes
{
    public interface IParameterTypeAppService : IApplicationService
    {
        Task<PagedResultDto<ParameterTypeListDto>> GetPagedAsync(GetParameterTypesInput input);

        Task<GetParameterTypeForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);

        Task CreateOrEditAsync(CreateOrUpdateParameterTypeInput input);

        Task DeleteAsync(EntityDto<Guid> input);
    }
}