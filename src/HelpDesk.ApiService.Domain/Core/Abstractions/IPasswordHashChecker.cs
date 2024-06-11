namespace HelpDesk.ApiService.Domain.Core.Abstractions
{
    public interface IPasswordHashChecker
    {
        bool HashesMatch(string passwordHash, string providedPassword);
    }
}
