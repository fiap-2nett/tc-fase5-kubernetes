namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record UserModel(int IdUser, string FullName, RoleModel Role);
}
