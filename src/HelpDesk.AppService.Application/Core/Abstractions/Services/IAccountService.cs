using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.Services
{
    public interface IAccountService
    {
        #region IAccountService Members

        Task<(bool IsSuccess, ErrorModel[] Errors)> PasswordSignInAsync(string username, string password, bool rememberMe);
        Task<(bool IsSuccess, ErrorModel[] Errors)> RegisterAsync(string name, string surname, string email, string password);

        Task<(bool IsSuccess, DetailedUserModel User, ErrorModel[] Errors)> GetAsync();
        Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(string name, string surname);
        Task<(bool IsSuccess, ErrorModel[] Errors)> ChangePasswordAsync(string password);

        #endregion
    }
}
