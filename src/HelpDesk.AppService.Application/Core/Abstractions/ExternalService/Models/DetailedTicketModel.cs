using System;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record DetailedTicketModel(int IdTicket, string Description,
        CategoryModel Category, StatusModel Status, DateTime CreatedAt, DateTime? LastUpdatedAt,
        int? LastUpdatedBy, string CancellationReason, int IdUserRequester, int? IdUserAssigned);
}
