namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record TokenResponseModel(string Token)
    {
        public string TokenName { get; set; }
    }
}
