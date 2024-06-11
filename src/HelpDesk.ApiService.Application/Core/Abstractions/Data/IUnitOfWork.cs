using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Data
{
    public interface IUnitOfWork
    {
        #region IUnitOfWork Members

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
