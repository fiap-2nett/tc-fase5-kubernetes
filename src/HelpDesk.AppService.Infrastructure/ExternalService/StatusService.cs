using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using Microsoft.Extensions.Logging;

namespace HelpDesk.AppService.Infrastructure.ExternalService
{
    internal sealed class StatusService : IStatusService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Constructors

        public StatusService(ILogger<StatusService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #endregion

        #region ICategoryService Members

        public async Task<IEnumerable<StatusModel>> GetAsync()
        {
            try
            {
                using var client = _httpClientFactory.CreateClient(typeof(IStatusService).FullName);
                return await client.GetFromJsonAsync<IEnumerable<StatusModel>>("ticketstatus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        #endregion
    }
}
