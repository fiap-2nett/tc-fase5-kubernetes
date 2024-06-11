using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.ApiService.Api.IntegrationTests.SeedWork;
using HelpDesk.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.ApiService.Api.IntegrationTests.Fixtures
{
    internal sealed class CategoryFixture
    {
        #region Read-Only Fields

        private readonly TestHostFixture _testHostFixture;

        #endregion

        #region Constructors

        public CategoryFixture(TestHostFixture testHostFixture)
        {
            _testHostFixture = testHostFixture;
        }

        #endregion

        #region Fixture Methods

        public async Task<(IEnumerable<Category> Categories, IEnumerable<Priority> Priorities)> SetFixtureAsync()
        {
            var categories = new List<Category>();
            var priorities = new List<Priority>();

            await _testHostFixture.ExecuteDbContextAsync(async dbContext =>
            {
                categories = await dbContext.Set<Category>()
                    .AsNoTracking()
                    .ToListAsync();

                priorities = await dbContext.Set<Priority>()
                    .AsNoTracking()
                    .ToListAsync();
            });

            return (categories, priorities);
        }

        #endregion
    }
}
