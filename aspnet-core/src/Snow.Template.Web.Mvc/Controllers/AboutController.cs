using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Snow.Template.Controllers;

namespace Snow.Template.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : TemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}



