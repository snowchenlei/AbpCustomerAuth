using System.Threading.Tasks;
using Abp.Application.Services;
using Snow.Template.Sessions.Dto;

namespace Snow.Template.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}



