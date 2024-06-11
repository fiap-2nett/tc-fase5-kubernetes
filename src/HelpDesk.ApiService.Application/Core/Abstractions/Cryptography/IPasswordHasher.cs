using HelpDesk.ApiService.Domain.ValueObjects;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Cryptography
{
    public interface IPasswordHasher
    {
        string HashPassword(Password password);
    }
}
