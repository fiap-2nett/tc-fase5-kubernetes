using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Extensions
{
    public static class ErrorExtensions
    {
        public static ErrorModel[] AsArray(this ErrorModel model)
            => new[] { model };
    }
}
