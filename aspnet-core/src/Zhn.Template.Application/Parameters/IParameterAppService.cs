using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zhn.Template.Parameters.Dto;

namespace Zhn.Template.Parameters
{
    public interface IParameterAppService : IApplicationService
    {
        Task<PagedResultDto<ParameterListDto>> GetParameters(GetParametersInput input);

        Task<GetParameterForEditOutput> GetParameterForEdit(NullableIdDto<Guid> input);

        Task<ListResultDto<ParameterTypeDto>> GetAllParameterTypes();

        Task CreateOrEditParameter(CreateOrUpdateParameterInput input);

        Task DeleteMenuItem(EntityDto<Guid> input);
    }
}