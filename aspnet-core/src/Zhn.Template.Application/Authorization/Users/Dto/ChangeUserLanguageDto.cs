using System.ComponentModel.DataAnnotations;

namespace Zhn.Template.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}

