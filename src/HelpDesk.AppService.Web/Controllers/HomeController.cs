using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using HelpDesk.AppService.Web.Extensions;
using HelpDesk.AppService.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AppService.Web.Controllers
{
    [Authorize]    
    public class HomeController : Controller
    {
        #region Read-Only Fields

        private readonly ITicketService _ticketService;

        #endregion

        #region Constructors

        public HomeController(ITicketService ticketService)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var model = await _ticketService.GetTicketsAsync();
            return View(model);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Tickets()
        {
            var model = await _ticketService.GetTicketsAsync();

            return Json(new
            {
                PartialView = await this.RenderViewToStringAsync("~/Views/Home/_Partials/_TableRows.cshtml", model?.Items)
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
