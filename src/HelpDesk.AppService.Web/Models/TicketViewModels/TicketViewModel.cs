using System.ComponentModel.DataAnnotations;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Web.Models.TicketViewModels
{
    public sealed class TicketViewModel
    {
        public int IdTicket { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int IdCategory { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        #region Operators

        public static implicit operator TicketViewModel(DetailedTicketModel model)
            => new TicketViewModel() { IdTicket = model.IdTicket, IdCategory = model.Category.IdCategory, Description = model.Description };

        #endregion
    }
}
