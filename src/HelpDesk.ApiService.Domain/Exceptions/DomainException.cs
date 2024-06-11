using System;
using HelpDesk.ApiService.Domain.Core.Primitives;

namespace HelpDesk.ApiService.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public Error Error { get; }

        public DomainException(Error error) : base(error.Message)
            => Error = error;
    }
}
