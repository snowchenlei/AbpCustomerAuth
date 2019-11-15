using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Authorization;
using Snow.Template.Authorization.Users;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Controllers;
using Snow.Template.Web.Models.Users;

namespace Snow.Template.Web.Controllers.Authorization
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UsersController : TemplateControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IMapper _mapper;

        public UsersController(IUserAppService userAppService
            , IMapper mapper)
        {
            _mapper = mapper;
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        //public async Task<JsonResult> Load(GetUsersInput input)
        //{
        //    var users = await _userAppService.GetUsers(input);
        //    return Json(users);
        //}

        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            var output = await _userAppService.GetForEditAsync(new NullableIdDto<long> { Id = id });
            var viewModel = _mapper.Map<CreateOrEditUserModalViewModel>(output);
            {
                //PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}