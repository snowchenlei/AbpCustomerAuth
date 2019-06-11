using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Parameters.Dto;

namespace Snow.Template.Parameters
{
    public interface IParameterAppService : IApplicationService
    {
        Task<PagedResultDto<ParameterListDto>> GetPaged(GetParametersInput input);

        Task<GetParameterForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        Task<ListResultDto<ParameterTypeDto>> GetAllParameterTypes();

        Task CreateOrEdit(CreateOrUpdateParameterInput input);

        Task Delete(EntityDto<Guid> input);
    }
}