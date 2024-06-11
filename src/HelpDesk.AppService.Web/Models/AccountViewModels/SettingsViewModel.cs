using System.ComponentModel.DataAnnotations;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Web.Models.AccountViewModels
{
    public class SettingsViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        #region Operators

        public static implicit operator SettingsViewModel(DetailedUserModel model)
            => new SettingsViewModel() { Name = model.Name, Surname = model.Surname };

        #endregion
    }
}
