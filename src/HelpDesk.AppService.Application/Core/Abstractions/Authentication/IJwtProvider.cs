using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace HelpDesk.AppService.Application.Core.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        #region IJwtProvider Members

        Task<TokenValidationResult> ValidateTokenAsync(string token);

        #endregion
    }
}
