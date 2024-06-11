using System.Collections.Generic;
using System.Linq;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelpDesk.AppService.Web.Extensions
{
    public static class ModelStateExtensions
    {
        #region Extension Methods

        public static void AddErrors(this ModelStateDictionary modelState, params ErrorModel[] errors)
            => errors.ForEach(error => modelState.AddModelError(string.Empty, error.Message));

        public static IEnumerable<ErrorModel> ToModel(this ModelStateDictionary modelState)
            => modelState.Values
                .SelectMany(entry => entry.Errors)
                ?.Select(error => new ErrorModel("", error.ErrorMessage));

        #endregion
    }
}
