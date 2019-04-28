using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Zhn.Template.Controllers;

namespace Zhn.Template.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : TemplateControllerBase
    {
        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}