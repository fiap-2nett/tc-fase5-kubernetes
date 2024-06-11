using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HelpDesk.AppService.Web.Extensions
{
    public static class ControllerExtensions
    {
        #region Extension Methods

        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrWhiteSpace(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            return await OnRenderViewToStringAsync(controller, viewNamePath);
        }

        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
        {
            if (string.IsNullOrWhiteSpace(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            return await OnRenderViewToStringAsync(controller, viewNamePath);
        }

        #endregion

        #region Private Methods

        private static async Task<string> OnRenderViewToStringAsync(Controller controller, string viewNamePath)
        {
            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                var result = viewNamePath.EndsWith(".cshtml")
                    ? viewEngine.GetView(viewNamePath, viewNamePath, false)
                    : viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                if (!result.Success)
                    throw new InvalidOperationException($"Unable to find view '{viewNamePath}'. Searched in: {string.Join(", ", result.SearchedLocations)}");

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    result.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await result.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}
