using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService
{
    public interface ICategoryService
    {
        #region ICategoryService Members

        Task<IEnumerable<CategoryModel>> GetAsync();

        #endregion
    }
}
