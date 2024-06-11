using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Core.Abstractions.Data;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Repositories;
using HelpDesk.ApiService.Domain.ValueObjects;
using HelpDesk.ApiService.Persistence.Core.Primitives;

namespace HelpDesk.ApiService.Persistence.Repositories
{
    internal sealed class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        #region Constructors

        public UserRepository(IDbContext dbContext)
            : base(dbContext)
        { }

        #endregion

        #region IUserRepository Members

        public async Task<User> GetByEmailAsync(Email email)
            => await FirstOrDefaultAsync(user => user.Email.Value == email);

        public async Task<bool> IsEmailUniqueAsync(Email email)
            => !await AnyAsync(user => user.Email.Value == email);

        #endregion
    }
}
