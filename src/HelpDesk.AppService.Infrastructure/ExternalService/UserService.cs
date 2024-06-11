using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Constants;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Extensions;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HelpDesk.AppService.Infrastructure.ExternalService
{
    internal sealed class UserService : IUserService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Constructors

        public UserService(ILogger<TicketService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #endregion

        #region IUserService Members

        public async Task<PagedListResponseModel<UserModel>> GetAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { nameof(page), page.ToString() },
                    { nameof(pageSize), pageSize.ToString() }
                };

                using var client = _httpClientFactory.CreateClient(typeof(IUserService).FullName);
                return await client.GetFromJsonAsync<PagedListResponseModel<UserModel>>(QueryHelpers.AddQueryString("users", parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<(bool IsSuccess, DetailedUserModel User, ErrorModel[] Errors)> GetMyProfileAsync()
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IUserService).FullName);
                var response = await client.GetAsync("users/me");

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<DetailedUserModel>();
                    return (true, successResponse, default);
                }

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponseModel>();
                return (false, default, errorResponse.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return (false, default, ErrorConstants.General.ServerError.AsArray());
        }

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> ChangePasswordAsync(string password)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IUserService).FullName);
                var response = await client.PutAsJsonAsync("users/me/change-password", new ChangePasswordRequestModel(password));

                if (response.IsSuccessStatusCode)
                    return (true, default);

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponseModel>();
                return (false, errorResponse.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return (false, ErrorConstants.General.ServerError.AsArray());
        }

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(string name, string surname)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IUserService).FullName);
                var response = await client.PutAsJsonAsync("users/me", new UpdateUserRequestModel(name, surname));

                if (response.IsSuccessStatusCode)
                    return (true, default);

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponseModel>();
                return (false, errorResponse.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return (false, ErrorConstants.General.ServerError.AsArray());
        }

        #endregion
    }
}
