using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Controllers;
using Snow.Template.Storage;

namespace Snow.Template.Web.Mvc.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tempFileCacheManager"></param>
        public ProfileController(
                ITempFileCacheManager tempFileCacheManager
            ) : base(tempFileCacheManager)
        {
        }
    }
}