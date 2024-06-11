using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService
{
    public interface IUserService
    {
        #region IUserService Members

        Task<PagedListResponseModel<UserModel>> GetAsync(int page = 1, int pageSize = 10);
        Task<(bool IsSuccess, DetailedUserModel User, ErrorModel[] Errors)> GetMyProfileAsync();

        Task<(bool IsSuccess, ErrorModel[] Errors)> ChangePasswordAsync(string password);
        Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(string name, string surname);

        #endregion
    }
}
