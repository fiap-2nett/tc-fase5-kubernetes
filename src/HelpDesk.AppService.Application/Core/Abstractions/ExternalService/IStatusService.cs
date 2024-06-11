using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService
{
    public interface IStatusService
    {
        #region IStatusService Members

        Task<IEnumerable<StatusModel>> GetAsync();

        #endregion
    }
}
