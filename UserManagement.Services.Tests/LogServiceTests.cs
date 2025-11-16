using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests
{
    public class LogServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddLogEntry()
        {
            // Arrange: Initialise object and set values 
            var service = new LogService();
            var logEntry = new UserLogEntry
            {
                UserId = 1,
                UserForename = "John",
                UserSurname = "Smith",
                Action = "Test Add"
            };

            // Act: Invokes the method under test with the arranged parameters
            await service.AddAsync(logEntry);
            var allLogs = await service.GetAllAsync();

            // Assert: Verify that the add method is creating a log
            allLogs.Should().ContainSingle().Which.Action
                .Should().Be("Test Add");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllLogs()
        {
            // Arrange 
            var service = new LogService();
            await service.AddAsync(new UserLogEntry { UserId = 1, Action = "A" });
            await service.AddAsync(new UserLogEntry { UserId = 2, Action = "B" });

            // Act
            var logs = await service.GetAllAsync();

            // Assert 
            logs.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnOnlyUserLogs()
        {
            // Arrange
            var service = new LogService();
            await service.AddAsync(new UserLogEntry { UserId = 1, Action = "A" });
            await service.AddAsync(new UserLogEntry { UserId = 2, Action = "B" });
            // Act
            var user1Logs = await service.GetByUserIdAsync(1);

            // Assert
            user1Logs.Should().HaveCount(1).And.OnlyContain(l => l.UserId == 1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectLogOrNull()
        {
            // Arrange
            var service = new LogService();
            await service.AddAsync(new UserLogEntry { UserId = 1, Action = "A" });
            var allLogs = await service.GetAllAsync();
            var firstLog = allLogs.First();

            // Act
            var logById = await service.GetByIdAsync(firstLog.Id);
            var missingLog = await service.GetByIdAsync(999);

            // Assert 
            logById.Should().NotBeNull();
            logById!.Id.Should().Be(firstLog.Id);
            missingLog.Should().BeNull();
        }
    }    
}   
