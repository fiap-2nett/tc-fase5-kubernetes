using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Constants;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Extensions;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Infrastructure.ExternalService.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelpDesk.AppService.Infrastructure.ExternalService
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly ServiceSettings _serviceSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Constructors

        public AuthenticationService(ILogger<AuthenticationService> logger,
            IOptions<ServiceSettings> options, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #endregion

        #region IAuthenticationService Members

        public async Task<(bool IsSuccess, TokenResponseModel Response, ErrorModel[] Errors)> GetAccessTokenAsync(string email, string password)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IAuthenticationService).FullName);
                var response = await client.PostAsJsonAsync("authentication/login", new TokenRequestModel(email, password));

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
                    successResponse.TokenName = _serviceSettings.TokenName;

                    return (true, successResponse, default);
                }
                
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponseModel>();
                return (false,null, errorResponse.Errors);                             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return (false, null, ErrorConstants.General.ServerError.AsArray());            
        }

        public async Task<(bool IsSuccess, TokenResponseModel Response, ErrorModel[] Errors)> RegisterAsync(string name, string surname, string email, string password)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IAuthenticationService).FullName);
                var response = await client.PostAsJsonAsync("authentication/register", new RegisterRequestModel(name, surname, email, password));

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
                    successResponse.TokenName = _serviceSettings.TokenName;

                    return (true, successResponse, default);
                }

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponseModel>();
                return (false, null, errorResponse.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return (false, null, ErrorConstants.General.ServerError.AsArray());
        }

        #endregion
    }
}
