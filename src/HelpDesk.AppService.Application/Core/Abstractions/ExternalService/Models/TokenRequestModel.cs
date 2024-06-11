namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record TokenRequestModel(string Email, string Password);
}
