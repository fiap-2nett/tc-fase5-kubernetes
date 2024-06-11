using System;
using System.Security.Claims;
using HelpDesk.AppService.Application.Core.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.AppService.Infrastructure.Authentication
{
    internal sealed class UserSessionProvider : IUserSessionProvider
    {
        #region IUserSessionProvider Members

        public int IdUser { get; }

        #endregion

        #region Constructors

        public UserSessionProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (!int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var idUser))
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            IdUser = idUser;
        }

        #endregion
    }
}
