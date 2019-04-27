using System.Threading.Tasks;
using Zhn.Template.Configuration.Dto;

namespace Zhn.Template.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}


