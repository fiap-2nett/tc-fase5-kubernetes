using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.Authentication;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Constants;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Extensions;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ExternalService = HelpDesk.AppService.Application.Core.Abstractions.ExternalService;

namespace HelpDesk.AppService.Application.Services
{
    public sealed class AccountService : IAccountService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly IJwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ExternalService.IUserService _userService;
        private readonly ExternalService.IAuthenticationService _authenticationService;

        #endregion

        #region Constructors

        public AccountService(
            ILogger<AccountService> logger,
            IJwtProvider jwtProvider,
            IHttpContextAccessor httpContextAccessor,
            ExternalService.IUserService userService,
            ExternalService.IAuthenticationService authenticationService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        #endregion

        #region IAccountService Members

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> PasswordSignInAsync(string username, string password, bool rememberMe)
        {
            var result = await _authenticationService.GetAccessTokenAsync(username, password);
            if (!result.IsSuccess)
                return (false, result.Errors);
            
            var validationResult = await _jwtProvider.ValidateTokenAsync(result.Response.Token);
            if (!validationResult.IsValid)
                return (false, ErrorConstants.Token.InvalidToken.AsArray());

            var securityToken = validationResult.SecurityToken as JwtSecurityToken;

            var isAuthenticated = await SignInAsync(securityToken.Claims, result.Response.Token, result.Response.TokenName, rememberMe);
            if (!isAuthenticated)
                return (false, ErrorConstants.Authentication.SignInError.AsArray());

            return (true, null);
        }

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> RegisterAsync(string name, string surname, string email, string password)
        {
            var result = await _authenticationService.RegisterAsync(name, surname, email, password);
            if (!result.IsSuccess)
                return (false, result.Errors);

            var validationResult = await _jwtProvider.ValidateTokenAsync(result.Response.Token);
            if (!validationResult.IsValid)
                return (false, ErrorConstants.Token.InvalidToken.AsArray());

            var securityToken = validationResult.SecurityToken as JwtSecurityToken;

            var isAuthenticated = await SignInAsync(securityToken.Claims, result.Response.Token, result.Response.TokenName, isPersistent: false);
            if (!isAuthenticated)
                return (false, ErrorConstants.Authentication.SignInError.AsArray());

            return (true, null);
        }

        public async Task<(bool IsSuccess, DetailedUserModel User, ErrorModel[] Errors)> GetAsync()
            => await _userService.GetMyProfileAsync();

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(string name, string surname)
            => await _userService.UpdateAsync(name, surname);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> ChangePasswordAsync(string password)
            => await _userService.ChangePasswordAsync(password);

        #endregion

        #region Private Methods

        private async Task<bool> SignInAsync(IEnumerable<Claim> claims, string token, string tokenName, bool isPersistent)
        {
            try
            {
                var principal = new ClaimsPrincipal(new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name,
                    ClaimTypes.Role
                ));

                var authenticationToken = new AuthenticationToken { Name = tokenName, Value = token };
                var props = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                props.StoreTokens(new[] { authenticationToken });

                await _httpContextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        #endregion
    }
}
