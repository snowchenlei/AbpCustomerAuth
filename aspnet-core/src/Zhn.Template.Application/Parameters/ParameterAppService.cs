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

        public ParameterAppService(IRepository<Parameter, Guid> parameteRepository, IMapper mapper)
        {
            _parameteRepository = parameteRepository;
            _mapper = mapper;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Parameters, PermissionNames.Pages_Administration_Parameters)]
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
        public async Task<GetParameterForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            return null;
        }
    }
}