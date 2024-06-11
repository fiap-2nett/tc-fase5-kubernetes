using System;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models
{
    public sealed record DetailedUserModel(int IdUser, string Name, string Surname, string Email, RoleModel Role, DateTime CreatedAt, DateTime? LastUpdatedAt);
}
