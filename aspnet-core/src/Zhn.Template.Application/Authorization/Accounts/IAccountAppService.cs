using System.Threading.Tasks;
using Abp.Application.Services;
using Zhn.Template.Authorization.Accounts.Dto;

namespace Zhn.Template.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}


