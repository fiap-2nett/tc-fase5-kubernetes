using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Contracts.Common;
using HelpDesk.ApiService.Application.Contracts.Tickets;
using HelpDesk.ApiService.Domain.Enumerations;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Services
{
    public interface ITicketService
    {
        #region ITicketService Members

        Task<DetailedTicketResponse> GetTicketByIdAsync(int idTicket, int idUserPerformedAction);
        Task<PagedList<TicketResponse>> GetTicketsAsync(GetTicketsRequest request, int idUserPerformedAction);
        Task<int> CreateAsync(int idCategory, string description, int idUserRequester);
        Task UpdateAsync(int idTicket, int idCategory, string description, int idUserPerformedAction);
        Task ChangeStatusAsync(int idTicket, TicketStatuses changedStatus, int idUserPerformedAction);
        Task CancelAsync(int idTicket, string cancellationReason, int idUserPerformedAction);
        Task AssignToUserAsync(int idTicket, int idUserAssigned, int idUserPerformedAction);
        Task CompleteAsync(int idTicket, int idUserPerformedAction);

        #endregion
    }
}
