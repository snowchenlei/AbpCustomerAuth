﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization;
using Zhn.Template.Controllers;
using Zhn.Template.Parameters;
using Zhn.Template.Web.Models.Parameters;

namespace Zhn.Template.Web.Mvc.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Parameters)]
    public class ParametersController : TemplateControllerBase
    {
        private readonly IParameterAppService _parameterAppService;

        public ParametersController(IParameterAppService parameterAppService)
        {
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
            var output = await _parameterAppService.GetParameterForEdit(new NullableIdDto<Guid> { Id = id });
            var viewModel = new CreateOrEditParameterModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}