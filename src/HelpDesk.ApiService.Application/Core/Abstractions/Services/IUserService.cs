using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Contracts.Authentication;
using HelpDesk.ApiService.Application.Contracts.Common;
using HelpDesk.ApiService.Application.Contracts.Users;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Services
{
    public interface IUserService
    {
        #region IUserService Members

        Task<DetailedUserResponse> GetUserByIdAsync(int idUser);
        Task<PagedList<UserResponse>> GetUsersAsync(GetUsersRequest request);
        Task<TokenResponse> CreateAsync(string name, string surname, string email, string password);
        Task ChangePasswordAsync(int idUser, string password);
        Task UpdateUserAsync(int idUser, string name, string surname);

        #endregion
    }
}
