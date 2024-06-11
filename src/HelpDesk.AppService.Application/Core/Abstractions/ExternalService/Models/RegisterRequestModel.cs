namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record RegisterRequestModel(string Name, string Surname, string Email, string Password);
}
