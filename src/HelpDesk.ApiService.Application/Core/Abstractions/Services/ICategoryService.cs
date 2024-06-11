using System.Threading.Tasks;
using System.Collections.Generic;
using HelpDesk.ApiService.Application.Contracts.Category;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Services
{
    public interface ICategoryService
    {
        #region ICategoryService Members

        Task<IEnumerable<CategoryResponse>> GetAsync();
        Task<DetailedCategoryResponse> GetByIdAsync(int idCategory);

        #endregion
    }
}
