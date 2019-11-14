using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Snow.Template.Controllers;
using Snow.Template.Authorization.Users;
using Abp.Runtime.Session;
using Snow.Template.Web.Models.Home;

namespace Snow.Template.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : TemplateControllerBase
    {
        private readonly IAbpSession abpSession;
        private readonly UserManager userManager;

        public HomeController(
            IAbpSession abpSession
            , UserManager userManager)
        {
            this.abpSession = abpSession;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            User user = userManager.GetUserById(abpSession.UserId.Value);
            var model = new LoginInfoModel
            {
                Name = user.Name,
                HeadImage = "/lib/AdminLTE/dist/img/user2-160x160.jpg",//abpSession.GetUserHeadImage(),
            };
            return View(model);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}