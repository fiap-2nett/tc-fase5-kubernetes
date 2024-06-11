using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HelpDesk.ApiService.Application.Core.Abstractions.Data;
using HelpDesk.ApiService.Application.Services;
using HelpDesk.ApiService.Domain.Entities;
using HelpDesk.ApiService.Domain.Enumerations;
using HelpDesk.ApiService.Domain.Extensions;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HelpDesk.ApiService.Application.UnitTests.Scenarios
{
    public sealed class TicketStatusServiceTests
    {
        #region Read-Only Fields

        private readonly Mock<IDbContext> _dbContextMock;

        #endregion

        #region Constructors

        public TicketStatusServiceTests()
        {
            _dbContextMock = new();
        }

        #endregion

        #region Unit Tests

        #region GetAsync

        [Fact]
        public async Task GetAsync_Should_ReturnStatusResponseEnumerableAsync()
        {
            // Arrange            
            _dbContextMock.Setup(x => x.Set<TicketStatus, byte>()).ReturnsDbSet(TicketStatusList());

            var ticketStatusService = new TicketStatusService(_dbContextMock.Object);

            // Act
            var testResult = await ticketStatusService.GetAsync();

            // Assert
            testResult.Should().NotBeNull();
            testResult.Should().HaveCount(TicketStatusList().Count());
            testResult.All(c => c.IdStatus > 0).Should().BeTrue();
            testResult.All(c => !c.Name.IsNullOrWhiteSpace()).Should().BeTrue();
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_Should_ReturnStatusResponseAsync()
        {
            // Arrange
            _dbContextMock.Setup(x => x.Set<TicketStatus, byte>()).ReturnsDbSet(TicketStatusList());

            var expectedResult = TicketStatusList().FirstOrDefault();
            var ticketStatusService = new TicketStatusService(_dbContextMock.Object);

            // Act
            var testResult = await ticketStatusService.GetByIdAsync(expectedResult.Id);

            // Assert
            testResult.Should().NotBeNull();
            testResult.IdStatus.Should().Be(expectedResult.Id);
            testResult.Name.Should().Be(expectedResult.Name);
        }

        #endregion

        #endregion

        #region Private Methods

        private IEnumerable<TicketStatus> TicketStatusList()
        {
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.New, name: "Novo");
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.Assigned, name: "Atribuído");
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.InProgress, name: "Em andamento");
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.OnHold, name: "Em espera");
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.Completed, name: "Concluído");
            yield return new TicketStatus(idTicketStatus: (byte)TicketStatuses.Cancelled, name: "Cancelado");
        }

        #endregion
    }
}
