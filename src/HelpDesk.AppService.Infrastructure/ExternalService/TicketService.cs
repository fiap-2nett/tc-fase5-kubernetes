using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Constants;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HelpDesk.AppService.Infrastructure.ExternalService
{
    internal sealed class TicketService : ITicketService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;        
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Constructors

        public TicketService(ILogger<TicketService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #endregion

        #region ITicketService Members

        public async Task<PagedListResponseModel<TicketModel>> GetAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { nameof(page), page.ToString() },
                    { nameof(pageSize), pageSize.ToString() } 
                };

                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                return await client.GetFromJsonAsync<PagedListResponseModel<TicketModel>>(QueryHelpers.AddQueryString("tickets", parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<(bool IsSuccess, DetailedTicketModel Ticket, ErrorModel[] Errors)> GetByIdAsync(int idTicket)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.GetAsync($"tickets/{idTicket}");

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<DetailedTicketModel>();
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

        public async Task<(bool IsSuccess, int IdTicket, ErrorModel[] Errors)> CreateAsync(int idCategory, string description)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsJsonAsync("tickets", new CreateTikenRequestModel(idCategory, description));

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<int>();
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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(int idTicket, int idCategory, string description)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PutAsJsonAsync($"tickets/{idTicket}", new UpdateTicketRequestModel(idCategory, description));

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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToMeAsync(int idTicket)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsync($"tickets/{idTicket}/assign-to/me", default);

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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToAsync(int idTicket, int idUserAssigned)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsJsonAsync($"tickets/{idTicket}/assign-to", new AssignToRequestModel(idUserAssigned));

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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> CompleteAsync(int idTicket)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsync($"tickets/{idTicket}/complete", default);

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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> ChangeStatusAsync(int idTicket, byte idStatus)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsJsonAsync($"tickets/{idTicket}/change-status", new ChangeStatusRequestModel(idStatus));

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

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> CancelAsync(int idTicket, string cancellationReason)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(ITicketService).FullName);
                var response = await client.PostAsJsonAsync($"tickets/{idTicket}/cancel", new CancelTicketRequestModel(cancellationReason));

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
