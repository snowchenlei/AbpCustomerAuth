using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Auditing;
using Zhn.Template.Auditing.Dto;
using Zhn.Template.Authorization;
using Zhn.Template.Controllers;
using Zhn.Template.Web.Models.AuditLogs;

namespace Zhn.Template.Web.Mvc.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_AuditLogs)]

    public class AuditLogsController : TemplateControllerBase
    {

        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> EntityChangeDetailModal(EntityChangeListDto entityChangeListDto)
        {
            var output = await _auditLogAppService.GetEntityPropertyChanges(entityChangeListDto.Id);

            var viewModel = new EntityChangeDetailModalViewModel(output, entityChangeListDto);

            return PartialView("_EntityChangeDetailModal", viewModel);
        }
    }
}