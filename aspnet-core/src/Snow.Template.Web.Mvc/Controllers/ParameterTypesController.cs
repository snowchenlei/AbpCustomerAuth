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
using Snow.Template.ParameterManager.ParameterTypes;
using Snow.Template.Web.Models.ParameterTypes;

namespace Snow.Template.Web.Mvc.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Parameters)]
    public class ParameterTypesController : TemplateControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IParameterTypeAppService _parameterTypeAppService;

        public ParameterTypesController(IParameterTypeAppService parameterTypeAppService
            , IMapper mapper)
        {
            _mapper = mapper;
            _parameterTypeAppService = parameterTypeAppService;
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_ParameterTypes)]
        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_ParameterTypes_Create, PermissionNames.Pages_Administration_ParameterTypes_Edit)]
        public async Task<ActionResult> CreateOrEditModal(Guid? id)
        {
            var output = await _parameterTypeAppService.GetForEdit(new NullableIdDto<Guid> { Id = id });
            var viewModel = _mapper.Map<CreateOrEditParameterTypeModalViewModel>(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}