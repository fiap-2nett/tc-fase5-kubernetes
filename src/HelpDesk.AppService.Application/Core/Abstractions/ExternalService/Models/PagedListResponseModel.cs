using System.Collections.Generic;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record PagedListResponseModel<TModel>(int Page, int PageSize, int TotalCount,
        bool HasNextPage, bool HasPreviousPage, IEnumerable<TModel> Items)
        where TModel : class;

}
