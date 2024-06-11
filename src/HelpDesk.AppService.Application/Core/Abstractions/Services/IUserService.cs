using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.Services
{
    public interface IUserService
    {
        #region IUserService Members

        Task<IEnumerable<UserModel>> GetAnalysts();

        #endregion
    }
}
