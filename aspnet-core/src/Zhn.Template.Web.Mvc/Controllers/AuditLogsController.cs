using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization;
using Zhn.Template.Controllers;

namespace Zhn.Template.Web.Mvc.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_AuditLogs)]

    public class AuditLogsController : TemplateControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}