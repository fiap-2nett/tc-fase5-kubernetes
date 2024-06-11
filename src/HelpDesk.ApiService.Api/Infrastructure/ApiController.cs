using HelpDesk.ApiService.Api.Constants;
using HelpDesk.ApiService.Api.Contracts;
using HelpDesk.ApiService.Domain.Core.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.ApiService.Api.Infrastructure
{
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        #region Methods

        protected new IActionResult Ok(object value)
            => base.Ok(value);

        protected IActionResult BadRequest(Error error)
            => BadRequest(new ApiErrorResponse(error));

        protected new IActionResult NotFound()
            => NotFound(Errors.NotFoudError.Message);

        #endregion
    }
}
