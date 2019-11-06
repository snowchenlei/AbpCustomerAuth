using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes)]
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
        public async Task<PagedResultDto<ParameterTypeListDto>> GetPagedAsync(GetParameterTypesInput input)
        {
            var query = _parameterTypeRepository.GetAll();

            var userCount = await query.CountAsync();

            var parameterTypes = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            var parameterListDtos = _mapper.Map<List<ParameterTypeListDto>>(parameterTypes);

            return new PagedResultDto<ParameterTypeListDto>(
                userCount,
                parameterListDtos
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Create,
            PermissionNames.Pages_Administration_ParameterTypes_Edit)]
        public async Task<GetParameterTypeForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
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

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Create,
            PermissionNames.Pages_Administration_Parameters_Edit)]
        public async Task CreateOrEditAsync(CreateOrUpdateParameterTypeInput input)
        {
            if (input.ParameterType.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Create)]
        private async Task CreateAsync(CreateOrUpdateParameterTypeInput input)
        {
            ParameterType parameterType = _mapper.Map<ParameterType>(input.ParameterType);
            await _parameterTypeRepository.InsertAsync(parameterType);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Edit)]
        private async Task UpdateAsync(CreateOrUpdateParameterTypeInput input)
        {
            Debug.Assert(input.ParameterType.Id != null, "input.ParameterType.Id != null");
            ParameterType parameter = await _parameterTypeRepository.GetAsync(input.ParameterType.Id.Value);
            parameter = _mapper.Map(input.ParameterType, parameter);
            await _parameterTypeRepository.UpdateAsync(parameter);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Delete)]
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            await _parameterTypeRepository.DeleteAsync(input.Id);
        }
    }
}