using System.Threading.Tasks;
using Abp.Application.Services;
using Zhn.Template.Sessions.Dto;

namespace Zhn.Template.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}


