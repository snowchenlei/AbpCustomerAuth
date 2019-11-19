using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Snow.Template.Controllers;
using Snow.Template.Authorization.Users;
using Abp.Runtime.Session;
using Snow.Template.Web.Models.Home;
using Snow.Template.Authorization.Users.Dto;

namespace Snow.Template.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : TemplateControllerBase
    {
        private readonly IAbpSession abpSession;
        private readonly UserManager userManager;
        private readonly IUserAppService _userAppservice;

        public HomeController(
            IAbpSession abpSession
            , UserManager userManager
            , IUserAppService userAppservice)
        {
            this.abpSession = abpSession;
            this.userManager = userManager;
            _userAppservice = userAppservice;
        }

        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            User user = await userManager.GetUserByIdAsync(abpSession.UserId.Value);
            GetHeadImageOutput headImage = await _userAppservice.GetHeadImageAsync();
            var model = new LoginInfoModel
            {
                Name = user.Name,
                HeadImage = headImage.HeadImagePath
            };
            return View(model);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}