using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization;
using Snow.Template.Authorization.Users;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Parameters.Dto;

namespace Snow.Template.Parameters
{
    public class ParameterAppService : TemplateAppServiceBase, IParameterAppService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Parameter, Guid> _parameterRepository;
        private readonly IRepository<ParameterType, Guid> _parameterTypeRepository;

        public ParameterAppService(IRepository<Parameter, Guid> parameterRepository, IMapper mapper,
            IRepository<ParameterType, Guid> parameterTypeRepository)
        {
            _parameterRepository = parameterRepository;
            _mapper = mapper;
            _parameterTypeRepository = parameterTypeRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters)]
        public async Task<PagedResultDto<ParameterListDto>> GetParameters(GetParametersInput input)
        {
            var query = _parameterRepository.GetAll()
                .WhereIf(input.ParameterTypeId.HasValue, p => p.ParameterType.Id == input.ParameterTypeId.Value);

            var userCount = await query.CountAsync();

            var users = await query
                .Include(p => p.ParameterType)
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var parameterListDtos = _mapper.Map<List<ParameterListDto>>(users);

            return new PagedResultDto<ParameterListDto>(
                userCount,
                parameterListDtos
            );
        }

        /// <summary>
        /// 获取用户修改信息
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Create,
            PermissionNames.Pages_Administration_Parameters_Edit)]
        public async Task<GetParameterForEditOutput> GetParameterForEdit(NullableIdDto<Guid> input)
        {
            GetParameterForEditOutput parameterOutput = new GetParameterForEditOutput();
            List<ParameterType> types = await _parameterTypeRepository.GetAll().AsNoTracking().ToListAsync();
            parameterOutput.ParameterTypes = _mapper.Map<List<ParameterTypeSelectListDto>>(types);
            if (input.Id.HasValue)
            {
                Parameter parameter = await _parameterRepository.GetAll()
                    .Include(p => p.ParameterType)
                    .FirstOrDefaultAsync(p => p.Id == input.Id.Value);
                foreach (ParameterTypeSelectListDto item in parameterOutput.ParameterTypes)
                {
                    if (item.Id == parameter.ParameterType.Id)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }

                parameterOutput.Parameter = _mapper.Map<ParameterEditDto>(parameter);
            }
            else
            {
                parameterOutput.Parameter = new ParameterEditDto();
            }

            return parameterOutput;
        }

        public async Task<ListResultDto<ParameterTypeDto>> GetAllParameterTypes()
        {
            List<ParameterType> types = await _parameterTypeRepository.GetAllListAsync();
            return new ListResultDto<ParameterTypeDto>(
                ObjectMapper.Map<List<ParameterTypeDto>>(types)
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Create,
            PermissionNames.Pages_Administration_Parameters_Edit)]
        public async Task CreateOrEditParameter(CreateOrUpdateParameterInput input)
        {
            if (input.Parameter.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Create)]
        private async Task CreateAsync(CreateOrUpdateParameterInput input)
        {
            Parameter parameter = _mapper.Map<Parameter>(input.Parameter);
            parameter.ParameterType = await _parameterTypeRepository.GetAsync(input.Parameter.ParameterTypeId);
            await _parameterRepository.InsertAsync(parameter);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Edit)]
        private async Task UpdateAsync(CreateOrUpdateParameterInput input)
        {
            Debug.Assert(input.Parameter.Id != null, "input.Parameter.Id != null");
            Parameter parameter = await _parameterRepository.GetAll()
                .Include(p => p.ParameterType)
                .FirstOrDefaultAsync(p => p.Id == input.Parameter.Id.Value); ;
            Parameter currentParameter = _mapper.Map(input.Parameter, parameter);
            if (parameter.ParameterType.Id != input.Parameter.ParameterTypeId)
            {
                currentParameter.ParameterType = await _parameterTypeRepository.GetAsync(input.Parameter.ParameterTypeId);
            }

            await _parameterRepository.UpdateAsync(currentParameter);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Delete)]
        public async Task DeleteMenuItem(EntityDto<Guid> input)
        {
            await _parameterRepository.DeleteAsync(input.Id);
        }
    }
}
