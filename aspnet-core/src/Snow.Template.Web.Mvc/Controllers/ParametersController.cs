using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Authorization;
using Snow.Template.Controllers;
using Snow.Template.Parameters;
using Snow.Template.Web.Models.Parameters;

namespace Snow.Template.Web.Mvc.Controllers
{
    /// <summary>
    /// 参数
    /// </summary>
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Parameters)]
    public class ParametersController : TemplateControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IParameterAppService _parameterAppService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="parameterAppService"></param>
        /// <param name="mapper"></param>
        public ParametersController(IParameterAppService parameterAppService
            , IMapper mapper)
        {
            _mapper = mapper;
            _parameterAppService = parameterAppService;
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Parameters)]
        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Parameters_Create, PermissionNames.Pages_Administration_Parameters_Edit)]
        public async Task<ActionResult> CreateOrEditModal(Guid? id)
        {
            var output = await _parameterAppService.GetForEdit(new NullableIdDto<Guid> { Id = id });
            var viewModel = _mapper.Map<CreateOrEditParameterModalViewModel>(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}