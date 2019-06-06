using System;
using System.Collections.Generic;
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
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.Users;
using Zhn.Template.Authorization.Users.Dto;
using Zhn.Template.Parameters.Dto;

namespace Zhn.Template.Parameters
{
    public class ParameterAppService : TemplateAppServiceBase, IParameterAppService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Parameter, Guid> _parameteRepository;
        private readonly IRepository<ParameterType, Guid> _parameterTypeRepository;

        public ParameterAppService(IRepository<Parameter, Guid> parameteRepository, IMapper mapper, IRepository<ParameterType, Guid> parameterTypeRepository)
        {
            _parameteRepository = parameteRepository;
            _mapper = mapper;
            _parameterTypeRepository = parameterTypeRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters)]
        public async Task<PagedResultDto<ParameterListDto>> GetParameters(GetParametersInput input)
        {
            var query = _parameteRepository.GetAll();

            var userCount = await query.CountAsync();

            var users = await query
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
        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters_Create, PermissionNames.Pages_Administration_Parameters_Edit)]
        public async Task<GetParameterForEditOutput> GetParameterForEdit(NullableIdDto<Guid> input)
        {
            GetParameterForEditOutput parameterOutput = new GetParameterForEditOutput();
            if (input.Id.HasValue)
            {
                Parameter parameter = await _parameteRepository.GetAsync(input.Id.Value);
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
    }
}