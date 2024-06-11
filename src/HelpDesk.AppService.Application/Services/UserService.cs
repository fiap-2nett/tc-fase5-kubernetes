using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;
using ExternalService = HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Enumerations;

namespace HelpDesk.AppService.Application.Services
{
    internal sealed class UserService : IUserService
    {
        #region Read-Only Fields

        private readonly ILogger _logger;

        private readonly ExternalService.IUserService _userService;

        #endregion

        #region Constructors

        public UserService(
            ILogger<AccountService> logger,
            ExternalService.IUserService userService,
            ExternalService.IStatusService statusService,
            ExternalService.ICategoryService categoryService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));            
        }

        #endregion

        #region IUserService Members

        public async Task<IEnumerable<UserModel>> GetAnalysts()
            => (await _userService.GetAsync(pageSize: int.MaxValue))?.Items
                .Where(user => user.Role.IdRole == (int)UserRoles.Analyst)
                .OrderBy(user => user.FullName)
                .ToList();

        #endregion
    }
}
