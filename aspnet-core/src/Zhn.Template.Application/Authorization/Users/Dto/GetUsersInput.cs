﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using Zhn.Template.Dto;

namespace Zhn.Template.Authorization.Users.Dto
{
    public class GetUsersInput : PagedAndSortedInputDto, IShouldNormalize, IGetUsersInput
    {
        public string Permission { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }
        }
    }
}
