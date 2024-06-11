using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Enumerations;
using HelpDesk.ApiService.Domain.ValueObjects;

namespace HelpDesk.ApiService.Application.UnitTests.TestEntities
{
    internal class UserTest : User
    {
        public UserTest(int idUser, string name, string surname, Email email, UserRoles userRole, string passwordHash)
            : base(name, surname, email, userRole, passwordHash)
        {
            Id = idUser;
        }
    }
}
