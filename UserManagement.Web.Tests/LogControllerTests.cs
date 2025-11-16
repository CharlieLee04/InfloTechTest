using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data.Tests
{
    public class LogsControllerTests
    {
        private readonly Mock<ILogService> _logService = new();

        private LogsController CreateController() => new(_logService.Object);

        [Fact]
        public async Task List_ShouldReturnAllLogsInView()
        {
            // Arrange: Initializes objects and sets the value of the data that is passed to the method under test
            var controller = CreateController();
            var logs = new[]
            {
                new UserLogEntry { Id = 1, UserForename = "John", UserSurname = "Doe", Action = "Create" },
                new UserLogEntry { Id = 2, UserForename = "Jane", UserSurname = "Smith", Action = "Edit" }
            };
            _logService.Setup(s => s.GetAllAsync()).ReturnsAsync(logs);

            // Act: Invokes the method under test with the arranged parameters
            var result = await controller.List() as ViewResult;

            // Assert: Verifies that the action of the method under test behaves as expected
            result.Should().NotBeNull();
            var model = result!.Model.Should().BeAssignableTo<IEnumerable<LogEntryViewModel>>().Subject;
            model.Should().HaveCount(2);
        }

        [Fact]
        public async Task Details_ExistingId_ShouldReturnLog()
        {
            // Arrange
            var controller = CreateController();
            var log = new UserLogEntry { Id = 1, UserForename = "John", UserSurname = "Doe", Action = "Create" };
            _logService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(log);

            // Act
            var result = await controller.Details(1) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            var model = result!.Model.Should().BeOfType<LogEntryViewModel>().Subject;
            model.Id.Should().Be(1);
            model.Action.Should().Be("Create");
        }

        [Fact]
        public async Task Details_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var controller = CreateController();
            _logService.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((UserLogEntry?)null);

            // Act
            var result = await controller.Details(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
