using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Contracts.Authentication;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Services
{
    public interface IAuthenticationService
    {
        #region IAuthenticationService Members

        Task<TokenResponse> Login(string email, string password);

        #endregion
    }
}
