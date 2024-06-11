using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HelpDesk.AppService.Infrastructure.ExternalService.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HelpDesk.AppService.Infrastructure.ExternalService.Handlers
{
    internal sealed class MessageHandler : DelegatingHandler
    {
        private readonly ServiceSettings _serviceSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageHandler(IOptions<ServiceSettings> options, IHttpContextAccessor httpContextAccessor)
        {
            _serviceSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync(_serviceSettings.TokenName);
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
