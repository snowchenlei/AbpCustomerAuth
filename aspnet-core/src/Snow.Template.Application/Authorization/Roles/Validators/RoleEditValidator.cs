using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization.Roles;
using FluentValidation;
using Snow.Template.Authorization.Roles.Dto;

namespace Snow.Template.Authorization.Roles.Validators
{
    public class RoleEditValidator : TemplateValidator<RoleEditDto>
    {
        public RoleEditValidator()
        {
            RuleFor(u => u.DisplayName)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(AbpRoleBase.MaxDisplayNameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
        }
    }
}

