using HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Models;

namespace HelpDesk.AppService.Application.Core.Abstractions.ExternalService.Constants
{
    public static class ErrorConstants
    {
        public static class General
        {
            public static ErrorModel ServerError => new ErrorModel(
                "ExternalService.ServerError",
                "The server encountered an unrecoverable error.");
        }

        public static class Token
        {
            public static ErrorModel InvalidToken => new ErrorModel(
                "ExternalService.InvalidToken",
                "The token did not pass the validation criteria.");
        }

        public static class Authentication
        {
            public static ErrorModel SignInError => new ErrorModel(
                "ExternalService.SignInError",
                "An error occurred while authenticating the user.");
        }
    }
}
