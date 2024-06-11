using HelpDesk.ApiService.Application.Core.Abstractions.Data;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Repositories;
using HelpDesk.ApiService.Persistence.Core.Primitives;

namespace HelpDesk.ApiService.Persistence.Repositories
{
    internal sealed class TicketRepository : GenericRepository<Ticket, int>, ITicketRepository
    {
        #region Constructors

        public TicketRepository(IDbContext dbContext)
            : base(dbContext)
        { }

        #endregion
    }
}
