using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization.Users;
using FluentValidation;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Authorization.MenuItems.Validators
{
    public class MenuItemEditValidator : TemplateValidator<MenuItemEditDto>
    {
        public MenuItemEditValidator()
        {
            RuleFor(m => m.Name)
                .NotNull()
                .WithMessage(L("IsRequired", "{PropertyName}"))
                .NotEmpty()
                .WithMessage(L("IsRequired", "{PropertyName}"));
        }
    }
}
