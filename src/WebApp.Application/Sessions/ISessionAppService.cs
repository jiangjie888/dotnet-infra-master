using System.Threading.Tasks;
using Abp.Application.Services;
using WebApp.Application.Sessions.Dto;

namespace WebApp.Application.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
