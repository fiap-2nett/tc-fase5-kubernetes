using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.Authentication;
using HelpDesk.AppService.Infrastructure.Authentication.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HelpDesk.AppService.Infrastructure.Authentication
{
    internal sealed class JwtProvider : IJwtProvider
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly JwtSettings _jwtSettings;

        #endregion

        #region Constructors

        public JwtProvider(ILogger<JwtProvider> logger, IOptions<JwtSettings> jwtOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtSettings = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        #endregion

        #region IJwtProvider Members

        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            TokenValidationResult validationResult = new();

            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentNullException(nameof(token));
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey))
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();                
                validationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                validationResult.IsValid = false;
                validationResult.Exception = ex;
            }

            return validationResult;            
        }

        #endregion
    }
}
