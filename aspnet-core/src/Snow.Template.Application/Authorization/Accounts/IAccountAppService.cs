using System.Threading.Tasks;
using Abp.Application.Services;
using Snow.Template.Authorization.Accounts.Dto;

namespace Snow.Template.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}



