using System;
using System.Collections.Generic;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Enumerations;
using HelpDesk.ApiService.Persistence.Core.Primitives;

namespace HelpDesk.ApiService.Persistence.Seeds
{
    internal sealed class RoleSeed : EntitySeedConfiguration<Role, byte>
    {
        public override IEnumerable<object> Seed()
        {
            yield return new { Id = (byte)UserRoles.Administrator, Name = "Administrador", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
            yield return new { Id = (byte)UserRoles.General, Name = "Geral", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
            yield return new { Id = (byte)UserRoles.Analyst, Name = "Analista", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
        }
    }
}
