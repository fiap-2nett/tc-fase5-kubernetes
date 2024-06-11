using System;

namespace HelpDesk.ApiService.Domain.Extensions
{
    public static class GuidExtensions
    {
        #region Extension Methods

        public static bool IsEmpty(this Guid source)
            => source == Guid.Empty;

        #endregion
    }
}
