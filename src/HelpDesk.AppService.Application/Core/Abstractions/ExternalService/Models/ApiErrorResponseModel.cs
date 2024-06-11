namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record ApiErrorResponseModel(params ErrorModel[] Errors);
}
