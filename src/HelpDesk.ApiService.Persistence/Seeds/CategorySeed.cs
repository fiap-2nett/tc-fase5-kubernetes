using System;
using System.Collections.Generic;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Enumerations;
using HelpDesk.ApiService.Persistence.Core.Primitives;

namespace HelpDesk.ApiService.Persistence.Seeds
{
    internal sealed class CategorySeed : EntitySeedConfiguration<Category, int>
    {
        public override IEnumerable<object> Seed()
        {
            yield return new { Id = 1, IdPriority = (byte)Priorities.Criticial, Name = "Indisponibilidade", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
            yield return new { Id = 2, IdPriority = (byte)Priorities.High, Name = "Lentidão", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
            yield return new { Id = 3, IdPriority = (byte)Priorities.Medium, Name = "Requisição", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
            yield return new { Id = 4, IdPriority = (byte)Priorities.Low, Name = "Dúvida", IsDeleted = false, CreatedAt = DateTime.MinValue.Date };
        }
    }
}
