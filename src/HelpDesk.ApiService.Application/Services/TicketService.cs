using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.ApiService.Application.Contracts.Category;
using HelpDesk.ApiService.Application.Contracts.Common;
using HelpDesk.ApiService.Application.Contracts.Tickets;
using HelpDesk.ApiService.Application.Contracts.Users;
using HelpDesk.ApiService.Application.Core.Abstractions.Data;
using HelpDesk.ApiService.Application.Core.Abstractions.Services;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Enumerations;
using HelpDesk.ApiService.Domain.Errors;
using HelpDesk.ApiService.Domain.Exceptions;
using HelpDesk.ApiService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.ApiService.Application.Services
{
    internal sealed class TicketService : ITicketService
    {
        #region Read-Only Fields

        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion

        #region Constructors

        public TicketService(IDbContext dbContext, IUnitOfWork unitOfWork,
            ITicketRepository ticketRepository, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        #endregion

        #region ITicketService Members

        public async Task<DetailedTicketResponse> GetTicketByIdAsync(int idTicket, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticketResult = await (
                from ticket in _dbContext.Set<Ticket, int>().AsNoTracking()
                join status in _dbContext.Set<TicketStatus, byte>().AsNoTracking()
                    on ticket.IdStatus equals status.Id
                join category in _dbContext.Set<Category, int>().AsNoTracking()
                    on ticket.IdCategory equals category.Id
                join userRequester in _dbContext.Set<User, int>().AsNoTracking()
                    on ticket.IdUserRequester equals userRequester.Id
                join userAssigned in _dbContext.Set<User, int>().AsNoTracking()
                    on ticket.IdUserAssigned equals userAssigned.Id into tmp
                    from userAssigned in tmp.DefaultIfEmpty()
                where
                    ticket.Id == idTicket
                select new DetailedTicketResponse
                {
                    IdTicket = ticket.Id,
                    Description = ticket.Description,
                    Status = new StatusResponse { IdStatus = status.Id, Name = status.Name },
                    Category = new CategoryResponse { IdCategory = category.Id, Name = category.Name },
                    UserRequester = new UserResponse { IdUser = userRequester.Id, FullName = userRequester.FullName },
                    UserAssigned = userAssigned != null
                        ? new UserResponse { IdUser = userAssigned.Id, FullName = userAssigned.FullName }
                        : null,
                    CreatedAt = ticket.CreatedAt,
                    LastUpdatedAt = ticket.LastUpdatedAt,
                    LastUpdatedBy = ticket.LastUpdatedBy,
                    CancellationReason = ticket.CancellationReason
                }
            ).FirstOrDefaultAsync();

            if (ticketResult is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            if (userPerformedAction.IdRole == (byte)UserRoles.General && ticketResult.UserRequester.IdUser != userPerformedAction.Id)
                throw new InvalidPermissionException(DomainErrors.User.InvalidPermissions);

            if (userPerformedAction.IdRole == (byte)UserRoles.Analyst && ticketResult.UserRequester.IdUser != userPerformedAction.Id && ticketResult.UserAssigned?.IdUser != userPerformedAction.Id)
                throw new InvalidPermissionException(DomainErrors.User.InvalidPermissions);

            return ticketResult;
        }

        public async Task<PagedList<TicketResponse>> GetTicketsAsync(GetTicketsRequest request, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticketsQuery =
                from ticket in _dbContext.Set<Ticket, int>().AsNoTracking()
                join status in _dbContext.Set<TicketStatus, byte>().AsNoTracking()
                    on ticket.IdStatus equals status.Id
                join category in _dbContext.Set<Category, int>().AsNoTracking()
                    on ticket.IdCategory equals category.Id
                join userRequester in _dbContext.Set<User, int>().AsNoTracking()
                    on ticket.IdUserRequester equals userRequester.Id
                join userAssigned in _dbContext.Set<User, int>().AsNoTracking()
                    on ticket.IdUserAssigned equals userAssigned.Id into tmp
                    from userAssigned in tmp.DefaultIfEmpty()
                select new 
                {
                    IdTicket = ticket.Id,
                    ticket.Description,
                    Status = new StatusResponse { IdStatus = status.Id, Name = status.Name },
                    Category = new CategoryResponse { IdCategory = category.Id, Name = category.Name },
                    UserRequester = new UserResponse { IdUser = userRequester.Id, FullName = userRequester.FullName },
                    ticket.IdUserAssigned,
                    UserAssigned = userAssigned != null
                        ? new UserResponse { IdUser = userAssigned.Id, FullName = userAssigned.FullName }
                        : null
                }
            ;

            if (userPerformedAction.IdRole == (byte)UserRoles.General)
                ticketsQuery = ticketsQuery.Where(t => t.UserRequester.IdUser == userPerformedAction.Id);

            if (userPerformedAction.IdRole == (byte)UserRoles.Analyst)
                ticketsQuery = ticketsQuery.Where(t => t.UserRequester.IdUser == userPerformedAction.Id || (t.IdUserAssigned == null || t.IdUserAssigned == userPerformedAction.Id));

            var totalCount = await ticketsQuery.CountAsync();

            var ticketsReponsePage = await ticketsQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(item => new TicketResponse
                {
                    IdTicket = item.IdTicket,
                    Description = item.Description,
                    Status = item.Status,
                    Category = item.Category,
                    UserRequester = item.UserRequester,
                    UserAssigned = item.UserAssigned
                })
                .ToArrayAsync();

            return new PagedList<TicketResponse>(ticketsReponsePage, request.Page, request.PageSize, totalCount);
        }

        public async Task<int> CreateAsync(int idCategory, string description, int idUserRequester)
        {
            var userRequester = await _userRepository.GetByIdAsync(idUserRequester);
            if (userRequester is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var category = await _categoryRepository.GetByIdAsync(idCategory);
            if (category is null)
                throw new NotFoundException(DomainErrors.Category.NotFound);

            var ticket = new Ticket(category, description, userRequester);

            _ticketRepository.Insert(ticket);
            await _unitOfWork.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task UpdateAsync(int idTicket, int idCategory, string description, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var category = await _categoryRepository.GetByIdAsync(idCategory);
            if (category is null)
                throw new NotFoundException(DomainErrors.Category.NotFound);

            var ticket = await _ticketRepository.GetByIdAsync(idTicket);
            if (ticket is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            ticket.Update(category, description, userPerformedAction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelAsync(int idTicket, string cancellationReason, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticket = await _ticketRepository.GetByIdAsync(idTicket);
            if (ticket is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            ticket.Cancel(cancellationReason, userPerformedAction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int idTicket, TicketStatuses changedStatus, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticket = await _ticketRepository.GetByIdAsync(idTicket);
            if (ticket is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            ticket.ChangeStatus(changedStatus, userPerformedAction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignToUserAsync(int idTicket, int idUserAssigned, int idUserPerformedAction)
        {
            var userAssigned = await _userRepository.GetByIdAsync(idUserAssigned);
            if (userAssigned is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticket = await _ticketRepository.GetByIdAsync(idTicket);
            if (ticket is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            ticket.AssignTo(userAssigned, userPerformedAction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CompleteAsync(int idTicket, int idUserPerformedAction)
        {
            var userPerformedAction = await _userRepository.GetByIdAsync(idUserPerformedAction);
            if (userPerformedAction is null)
                throw new NotFoundException(DomainErrors.User.NotFound);

            var ticket = await _ticketRepository.GetByIdAsync(idTicket);
            if (ticket is null)
                throw new NotFoundException(DomainErrors.Ticket.NotFound);

            ticket.Complete(userPerformedAction);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}
