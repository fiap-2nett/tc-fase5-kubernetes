using System;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using HelpDesk.AppService.Web.Enumerators;
using HelpDesk.AppService.Web.Extensions;
using HelpDesk.AppService.Web.Models.TicketViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AppService.Web.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        #region Read-Only Fields

        private readonly ITicketService _ticketService;

        #endregion

        #region Constructors

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        public IActionResult Index()
            => View();

        [HttpGet]
        public async Task<IActionResult> Categories()
            => View(await _ticketService.GetCategoriesAsync());

        [HttpGet]
        public async Task<IActionResult> Status()
            => View(await _ticketService.GetTicketStatusAsync());

        [HttpGet]
        public async Task<IActionResult> GetActionModal(int ticketId, byte actionType)
        {
            var isSuccess = true;
            ErrorModel[] errors = default;
            var model = new TicketActionViewModel { IdTicket = ticketId, IdActionType = actionType };

            if (actionType != (byte)ActionType.Create)
            {
                var result = await _ticketService.GetByIdAsync(ticketId);
                if (result.IsSuccess)
                {
                    model.IdUserAssigned = result.Ticket?.IdUserAssigned;
                    model.IdCategory = result.Ticket.Category.IdCategory;
                    model.Description = result.Ticket.Description;
                }

                errors = result.Errors;
                isSuccess = result.IsSuccess;
            }

            return Json(new
            {
                Errors = errors,
                IsSuccess = isSuccess,
                PartialView = await this.RenderViewToStringAsync("~/Views/Ticket/_Partials/_ModalAction.cshtml", model)
            });
        }

        [HttpPost]        
        public async Task<IActionResult> DoActionTicket([FromRoute] int id, TicketActionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = (ActionType)model.IdActionType switch
                {
                    ActionType.Create => await _ticketService.CreateAsync(model.IdCategory, model.Description),
                    ActionType.Edit => await _ticketService.UpdateAsync(model.IdTicket, model.IdCategory, model.Description),
                    ActionType.AssignTo => await _ticketService.AssignToAsync(model.IdTicket, model.IdUserAssigned.Value),
                    ActionType.AssignToMe => await _ticketService.AssignToMeAsync(model.IdTicket),
                    ActionType.ChangeStatusTo => await _ticketService.ChangeStatusAsync(model.IdTicket, model.IdStatusChanged.Value),
                    ActionType.ChangeStatusToComplete => await _ticketService.CompleteAsync(model.IdTicket),
                    ActionType.Cancellation => await _ticketService.CancelAsync(model.IdTicket, model.CancellationReason),
                    _ => throw new ArgumentException("Invalid ActionType")
                };

                return Json(new
                {
                    result.IsSuccess,
                    result.Errors
                });
            }

            return Json(new
            {
                IsSuccess = false,
                Errors = ModelState.ToModel()
            });
        }

        #endregion        
    }
}
