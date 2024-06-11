namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record GetTicketRequestModel(int Page = 1, int PageSize = 10);
}
