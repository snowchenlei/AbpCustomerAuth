using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization.Users;
using FluentValidation;
using Snow.Template.ParameterManager.ParameterTypes.Dto;

namespace Snow.Template.ParameterManager.ParameterTypes.Validators
{
    public class ParameterTypeEditValidator : TemplateValidator<ParameterTypeEditDto>
    {
        public ParameterTypeEditValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(TemplateConsts.MaxNameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.Code)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(TemplateConsts.MaxCodeLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
        }
    }
}