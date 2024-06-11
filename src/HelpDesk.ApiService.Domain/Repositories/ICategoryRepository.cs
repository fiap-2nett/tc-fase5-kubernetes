using System.Threading.Tasks;
using HelpDesk.ApiService.Domain.Entities;

namespace HelpDesk.ApiService.Domain.Repositories
{
    public interface ICategoryRepository
    {
        #region ICategoryRepository Members

        Task<Category> GetByIdAsync(int idCategory);

        #endregion
    }
}
