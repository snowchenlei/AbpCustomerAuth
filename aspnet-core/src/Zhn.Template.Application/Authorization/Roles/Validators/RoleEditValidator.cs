using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization.Roles;
using FluentValidation;
using Zhn.Template.Authorization.Roles.Dto;

namespace Zhn.Template.Authorization.Roles.Validators
{
    public class RoleEditValidator : TemplateValidator<RoleEditDto>
    {
        public RoleEditValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(AbpRoleBase.MaxNameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
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
