﻿using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Snow.Template.Authorization.Roles;
using Snow.Template.Localization;

namespace Snow.Template.Authorization.Roles.Dto
{
    public class RoleEditDto : NullableIdDto<int>
    {
        [TemplateDisplayName("DisplayName")]
        public string DisplayName { get; set; }

        [TemplateDisplayName("Default")]
        public bool IsDefault { get; set; }
    }
}
