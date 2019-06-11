using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Snow.Template.Parameters.Dto;

namespace Snow.Template.ParameterManager.Parameters.Validators
{
    public class ParameterEditValidator : TemplateValidator<ParameterEditDto>
    {
        public ParameterEditValidator()
        {
            RuleFor(u => u.ParameterTypeId)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"));
            RuleFor(u => u.Sort)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"));
            RuleFor(u => u.Value)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(TemplateConsts.MaxValueLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.Description)
                .MaximumLength(TemplateConsts.MaxDescriptionLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
        }
    }
}