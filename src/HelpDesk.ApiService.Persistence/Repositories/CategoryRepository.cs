using HelpDesk.ApiService.Application.Core.Abstractions.Data;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Repositories;
using HelpDesk.ApiService.Persistence.Core.Primitives;

namespace HelpDesk.ApiService.Persistence.Repositories
{
    internal sealed class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        #region Constructors

        public CategoryRepository(IDbContext dbContext)
            : base(dbContext)
        { }

        #endregion
    }
}
