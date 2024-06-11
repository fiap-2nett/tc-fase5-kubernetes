using System;
using HelpDesk.ApiService.Domain.Core.Primitives;

namespace HelpDesk.ApiService.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public Error Error { get; }

        public NotFoundException(Error error) : base(error.Message)
            => Error = error;
    }
}
