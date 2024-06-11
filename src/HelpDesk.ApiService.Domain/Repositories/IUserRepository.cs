using System.Threading.Tasks;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.ValueObjects;

namespace HelpDesk.ApiService.Domain.Repositories
{
    public interface IUserRepository
    {
        #region IUserRepository Members

        Task<User> GetByIdAsync(int idUser);
        Task<User> GetByEmailAsync(Email email);
        Task<bool> IsEmailUniqueAsync(Email email);
        void Insert(User user);

        #endregion
    }
}
