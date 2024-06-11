using System;
using HelpDesk.ApiService.Domain.Core.Primitives;

namespace HelpDesk.ApiService.Domain.Exceptions
{
    public class InvalidPermissionException : Exception
    {
        public Error Error { get; }

        public InvalidPermissionException(Error error) : base(error.Message)
            => Error = error;
    }
}
