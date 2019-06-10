using System.ComponentModel.DataAnnotations;

namespace Snow.Template.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}


