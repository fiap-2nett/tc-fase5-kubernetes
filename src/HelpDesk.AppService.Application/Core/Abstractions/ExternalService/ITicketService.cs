using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService
{
    public interface ITicketService
    {
        #region ITicketService Members

        Task<PagedListResponseModel<TicketModel>> GetAsync(int page = 1, int pageSize = 10);
        Task<(bool IsSuccess, DetailedTicketModel Ticket, ErrorModel[] Errors)> GetByIdAsync(int idTicket);

        Task<(bool IsSuccess, int IdTicket, ErrorModel[] Errors)> CreateAsync(int idCategory, string description);
        Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(int idTicket, int idCategory, string description);

        Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToMeAsync(int idTicket);
        Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToAsync(int idTicket, int idUserAssigned);

        Task<(bool IsSuccess, ErrorModel[] Errors)> CompleteAsync(int idTicket);
        Task<(bool IsSuccess, ErrorModel[] Errors)> ChangeStatusAsync(int idTicket, byte idStatus);
        Task<(bool IsSuccess, ErrorModel[] Errors)> CancelAsync(int idTicket, string cancellationReason);

        #endregion
    }
}
