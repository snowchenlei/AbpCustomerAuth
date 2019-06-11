using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization.Users;
using Abp.Localization;
using Abp.Localization.Sources;
using FluentValidation;
using Snow.Template.Authorization.Users.Dto;

namespace Snow.Template.Authorization.Users.Validators
{
    public class UserEditValidator : TemplateValidator<UserEditDto>
    {
        public UserEditValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(AbpUserBase.MaxNameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.Surname)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(AbpUserBase.MaxSurnameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.UserName)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .MaximumLength(AbpUserBase.MaxUserNameLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.EmailAddress)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .EmailAddress()
                .WithMessage(L("EnterAProper", "{PropertyName}"))
                .MaximumLength(AbpUserBase.MaxEmailAddressLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.Password)
                .MaximumLength(AbpUserBase.MaxPasswordLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"));
            RuleFor(u => u.PhoneNumber)
                .MaximumLength(AbpUserBase.MaxPhoneNumberLength)
                .WithMessage(L("UnderMaxLength", "{PropertyName}", "{MaxLength}"))
                .Matches("^(((\\+\\d{2}-)?0\\d{2,3}-\\d{7,8})|((\\+\\d{2}-)?(\\d{2,3}-)?([1][3,4,5,7,8][0-9]\\d{8})))$")
                .WithMessage(L("EnterAProper", "{PropertyName}"));
        }
    }
}