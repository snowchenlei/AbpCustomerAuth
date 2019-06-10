using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization;
using Snow.Template.ParameterManager.ParameterTypes.Dto;
using Snow.Template.Parameters;

namespace Snow.Template.ParameterManager.ParameterTypes
{
    public class ParameterTypeAppService : TemplateAppServiceBase, IParameterTypeAppService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ParameterType, Guid> _parameterTypeRepository;

        public ParameterTypeAppService(IMapper mapper, IRepository<ParameterType, Guid> parameterTypeRepository)
        {
            _mapper = mapper;
            _parameterTypeRepository = parameterTypeRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes)]
        public async Task<PagedResultDto<ParameterTypeListDto>> GetPaged(GetParameterTypesInput input)
        {
            var query = _parameterTypeRepository.GetAll();

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var parameterListDtos = _mapper.Map<List<ParameterTypeListDto>>(users);

            return new PagedResultDto<ParameterTypeListDto>(
                userCount,
                parameterListDtos
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Create,
            PermissionNames.Pages_Administration_ParameterTypes_Edit)]
        public async Task<GetParameterTypeForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            GetParameterTypeForEditOutput parameterOutput = new GetParameterTypeForEditOutput();
            if (input.Id.HasValue)
            {
                ParameterType parameter = await _parameterTypeRepository.GetAsync(input.Id.Value);
                parameterOutput.ParameterType = _mapper.Map<ParameterTypeEditDto>(parameter);
            }
            else
            {
                parameterOutput.ParameterType = new ParameterTypeEditDto();
            }

            return parameterOutput;
        }
    }
}
