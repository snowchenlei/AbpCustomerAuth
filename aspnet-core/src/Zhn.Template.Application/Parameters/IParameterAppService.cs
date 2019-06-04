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
    }
}