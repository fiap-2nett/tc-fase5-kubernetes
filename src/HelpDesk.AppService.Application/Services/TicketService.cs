using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;
using ExternalService = HelpDesk.AppService.Application.Core.Abstractions.ExternalService;


namespace HelpDesk.AppService.Application.Services
{
    public sealed class TicketService : ITicketService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;

        private readonly ExternalService.ITicketService _ticketService;
        private readonly ExternalService.IStatusService _statusService;
        private readonly ExternalService.ICategoryService _categoryService;

        #endregion

        #region Constructors

        public TicketService(
            ILogger<AccountService> logger,
            ExternalService.ITicketService ticketService,
            ExternalService.IStatusService statusService,
            ExternalService.ICategoryService categoryService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        #endregion

        #region ITicketService Members

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
            => await _categoryService.GetAsync();

        public async Task<IEnumerable<StatusModel>> GetTicketStatusAsync()
            => await _statusService.GetAsync();

        public async Task<PagedListResponseModel<TicketModel>> GetTicketsAsync(int page = 1, int pageSize = int.MaxValue)
            => await _ticketService.GetAsync(page, pageSize);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> CreateAsync(int idCategory, string description)
        {
            var result = await _ticketService.CreateAsync(idCategory, description);
            return (result.IsSuccess, result.Errors);
        }

        public async Task<(bool IsSuccess, DetailedTicketModel Ticket, ErrorModel[] Errors)> GetByIdAsync(int idTicket)
            => await _ticketService.GetByIdAsync(idTicket);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> UpdateAsync(int idTicket, int idCategory, string description)
            => await _ticketService.UpdateAsync(idTicket, idCategory, description);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToMeAsync(int idTicket)
            => await _ticketService.AssignToMeAsync(idTicket);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> AssignToAsync(int idTicket, int idUserAssigned)
            => await _ticketService.AssignToAsync(idTicket, idUserAssigned);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> CompleteAsync(int idTicket)
            => await _ticketService.CompleteAsync(idTicket);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> ChangeStatusAsync(int idTicket, byte idStatus)
            => await _ticketService.ChangeStatusAsync(idTicket, idStatus);

        public async Task<(bool IsSuccess, ErrorModel[] Errors)> CancelAsync(int idTicket, string cancellationReason)
            => await _ticketService.CancelAsync(idTicket, cancellationReason);

        #endregion
    }
}
