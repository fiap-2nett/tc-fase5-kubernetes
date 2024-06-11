using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HelpDesk.AppService.Infrastructure.ExternalService.Handlers
{
    internal sealed class SkipServerCertificateValidationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var handler = InnerHandler;

            while (handler is DelegatingHandler)
            {
                handler = ((DelegatingHandler)handler).InnerHandler;
            }

            if (handler is HttpClientHandler httpClientHandler && httpClientHandler.ServerCertificateCustomValidationCallback is null)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
