using System.Threading.Tasks;
using HelpDesk.ApiService.Domain.Entities;

namespace HelpDesk.ApiService.Domain.Repositories
{
    public interface ITicketRepository
    {
        #region ITicketRepository Members

        Task<Ticket> GetByIdAsync(int idTicket);
        void Insert(Ticket ticket);

        #endregion
    }
}
