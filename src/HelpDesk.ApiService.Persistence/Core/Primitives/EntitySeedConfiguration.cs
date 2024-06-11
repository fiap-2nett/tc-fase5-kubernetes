using System;
using System.Collections.Generic;
using HelpDesk.ApiService.Domain.Core.Primitives;
using HelpDesk.ApiService.Persistence.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.ApiService.Persistence.Core.Primitives
{
    internal abstract class EntitySeedConfiguration<TEntity, TKey> : IEntitySeedConfiguration
        where TEntity : Entity<TKey>
        where TKey : IEquatable<TKey>
    {
        #region IEntitySeedConfiguration Members

        public abstract IEnumerable<object> Seed();

        public void Configure(ModelBuilder modelBuilder)
            => modelBuilder.Entity<TEntity>().HasData(Seed());

        #endregion
    }
}
