using System.Threading.Tasks;
using Snow.Template.Configuration.Dto;

namespace Snow.Template.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}



