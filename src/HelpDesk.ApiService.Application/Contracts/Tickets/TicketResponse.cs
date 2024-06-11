using HelpDesk.ApiService.Application.Contracts.Category;
using HelpDesk.ApiService.Application.Contracts.Users;

namespace HelpDesk.ApiService.Application.Contracts.Tickets
{
    /// <summary>
    /// Represents the ticket response.
    /// </summary>
    public sealed class TicketResponse
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        public int IdTicket { get; set; }

        /// <summary>
        /// Gets or sets the ticket description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ticket category.
        /// </summary>
        public CategoryResponse Category { get; set; }

        /// <summary>
        /// Gets or sets the ticket status.
        /// </summary>
        public StatusResponse Status { get; set; }

        /// <summary>
        /// Gets or sets the ticket user requester.
        /// </summary>
        public UserResponse UserRequester { get; set; }

        /// <summary>
        /// Gets or sets the ticket user assigned.
        /// </summary>
        public UserResponse UserAssigned { get; set; }
    }
}
