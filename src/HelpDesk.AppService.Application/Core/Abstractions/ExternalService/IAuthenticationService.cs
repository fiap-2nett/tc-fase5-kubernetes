using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService
{
    public interface IAuthenticationService
    {
        #region IAuthenticationService Members

        Task<(bool IsSuccess, TokenResponseModel Response, ErrorModel[] Errors)> GetAccessTokenAsync(string email, string password);
        Task<(bool IsSuccess, TokenResponseModel Response, ErrorModel[] Errors)> RegisterAsync(string name, string surname, string email, string password);

        #endregion
    }
}
