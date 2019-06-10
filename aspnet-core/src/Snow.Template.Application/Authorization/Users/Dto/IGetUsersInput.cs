using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Snow.Template.Authorization.Users.Dto
{
   public  interface IGetUsersInput : ISortedResultRequest
    {

        string Permission { get; set; }

        int? Role { get; set; }

        bool OnlyLockedUsers { get; set; }
    }
}

