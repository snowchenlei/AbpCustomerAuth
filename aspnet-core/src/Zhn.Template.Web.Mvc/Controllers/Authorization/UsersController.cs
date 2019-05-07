using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.Users;
using Zhn.Template.Authorization.Users.Dto;
using Zhn.Template.Controllers;
using Zhn.Template.Web.Models.Users;

namespace Zhn.Template.Web.Controllers.Authorization
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UsersController : TemplateControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Load(PagedUserResultRequestDto input)
        {
            var users = await _userAppService.GetAll(input);
            return Json(users);
        }

        public async Task<ActionResult> CreateOrEditModal(long? id)
        {
            var output = await _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
            var viewModel = new CreateOrEditUserModalViewModel(output)
            {
                //PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}