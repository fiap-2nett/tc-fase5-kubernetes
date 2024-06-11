using HelpDesk.ApiService.Domain.Entities;

namespace HelpDesk.ApiService.Application.Core.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        #region IJwtProvider Members

        string Create(User user);

        #endregion
    }
}
