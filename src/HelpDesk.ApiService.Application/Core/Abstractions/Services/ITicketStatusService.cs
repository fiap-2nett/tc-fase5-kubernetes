using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Contracts.Tickets;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Services
{
    public interface ITicketStatusService
    {
        #region ITicketStatusService Members

        Task<IEnumerable<StatusResponse>> GetAsync();
        Task<StatusResponse> GetByIdAsync(byte idTicketStatus);

        #endregion
    }
}
