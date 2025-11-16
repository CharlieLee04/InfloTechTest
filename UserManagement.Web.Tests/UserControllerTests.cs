using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List("active");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_FilterActive_ShouldReturnOnlyActiveUsers()
    {
        // Arrange: Create controller and mock users 
        var controller = CreateController();
        var users = new[]
        {
            new User { Id = 1, Forename = "ActiveUser", IsActive = true },
            new User { Id = 2, Forename = "Inactive User", IsActive = false }
        };

        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act: Call List with filter active
        var result = await controller.List("active");

        // Assert: Only the active user is included 
        var model = result.Model.Should().BeOfType<UserListViewModel>().Subject;
        model.Items.Should().OnlyContain(u => u.IsActive).And.HaveCount(1);
    }

    [Fact]
    public async Task CreatePost_ValidModel_ShouldRedirectToList()
    {
        // Arrange: Prepare the controller, input model, and mock behaviour.
        var controller = CreateController();
        var model = new CreateUserViewModel
        {
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        _userService.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act: Invokes the Create action with the arranged model.
        var result = await controller.Create(model);

        // Assert: verify that the action redirects to the List page when successful.
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public async Task EditGet_UserExists_ShouldReturnViewModel()
    {
        // Arrange: Prepare the controller and mock user data
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        // Act: Invokes the GET action with the user ID.
        var result = await controller.Edit(1);

        // Assert: Verify that the result is a ViewResult that contains the correct model.
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<EditUserViewModel>()
            .Which.Id.Should().Be(1);
    }
    
    [Fact]
    public async Task EditPost_ValidModel_ShouldUpdateUserAndRedirectToList()
    {
        // Arrange: Prepare the controller, input model, and mock user. 
        var controller = CreateController();
        var existingUser = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

        var model = new EditUserViewModel
        {
            Id = 1,
            Forename = "JohnUpdated",
            Surname = "SmithUpdated",
            Email = "johnUpdated@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = false
        };

        // Act: Invokes Edit POST action with the user ID and updated model.
        var result = await controller.Edit(1, model);

        // Assert: Verify that the action redirects to the List page when updated successfully. 
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public async Task Details_UserExists_ShouldReturnViewWithModel()
    {
        // Arrange: Prepare the controller and mock a user
        var controller = CreateController();
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        // Act: Invoke the Details action with the users ID.
        var result = await controller.Details(1);

        // Assert: verify that the result is a ViewResult containing the correct model
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserDetailsViewModel>()
                .Which.Id.Should().Be(1);
    }

    [Fact]
    public async Task DeleteUser_ValidId_ShouldRedirectToList()
    {
        // Arrange: Prepare the controller and mock the service so it returns true upon deletion.
        var controller = CreateController();
        var existingUser = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act: Invoke the DeleteUser POST action using the user ID
        var result = await controller.DeleteUser(1);

        // Assert: Verify that the action redirtects to the list page after successful deletion.
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    
    // The following tests will be for the Logging implementation in UserController 
    // This will consist of testing the logging when: Creating a user, Editing a user,
    // Deleting a user and that Details() will load the logs. 

    [Fact]
    public async Task Create_Post_WhenSuccessful_ShouldCreateLog()
    {
        // Arrange: Prepare the controller, model and user
        var controller = CreateController();

        var model = new CreateUserViewModel
        {
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        var user = new User
        {
            Id = 10,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            IsActive = model.IsActive
        };

        _userService.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(true).Callback<User>(u => u.Id = user.Id);

        // ACT: Invoke Create Post action
        var result = await controller.Create(model);

        // Assert: Verify that a single log has been made
        _logService.Verify(s => s.AddAsync(It.Is<UserLogEntry>(l => 
            l.UserId == 10 &&
            l.Action == "User Created")), Times.Once);
    }

    [Fact]
    public async Task Edit_Post_WhenDetailsChanged_ShouldCreateLog()
    {
        // Arrange: Prepare the controller, model and user 
        var controller = CreateController();

        var existingUser = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true  
        };

        var updatedModel = new EditUserViewModel
        {
            Id = 1,
            Forename = "JohnUpdated",
            Surname = "SmithUpdated",
            Email = "johnUpdated@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = false
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act: Invoke edit post aciton
        var result = await controller.Edit(1, updatedModel);

        // Assert: Ensure that a single log has been made for the edit action
        _logService.Verify(s => s.AddAsync(It.Is<UserLogEntry>(l =>
            l.UserId == 1 &&
            l.Action == "User Edited" &&
            l.Details!.Contains("Forename changed") &&
            l.Details.Contains("Email changed")
        )), Times.Once);
    }

    [Fact]
    public async Task Edit_Post_WhenNoChanges_ShouldNotLog()
    {
        // Arrange: Create controller, user and model
        var controller = CreateController();

        var existingUser = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true  
        };


        // Make model the same as existingUser
        var model = new EditUserViewModel
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true  
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _userService.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act: Invoke edit post action
        var result = await controller.Edit(1, model);

        // Assert: Ensure that no log was created
        _logService.Verify(s => s.AddAsync(It.IsAny<UserLogEntry>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenSuccessful_CreatesLog()
    {
        // Arrange: Create controller and user 
        var controller = CreateController();

        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "john@example.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true  
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);
        _userService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act: Invoke Post Delete action 
        var result = await controller.DeleteUser(1);

        // Assert: Verify that a single log has been created
        _logService.Verify(s => s.AddAsync(It.Is<UserLogEntry>(l =>
            l.UserId == 1 &&
            l.Action == "User Deleted"
        )), Times.Once);
    }

    [Fact]
    public async Task Details_ShouldLoadLogs()
    {
        // Arrange: Create controller, Mock user and logs
        var controller = CreateController();

        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith"
        };

        var logs = new[]
        {
            new UserLogEntry { UserId = 1, Action = "Test1" },
            new UserLogEntry { UserId = 1, Action = "Test2" }
        };

        _userService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);
        _logService.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync(logs);

        // Act: Invoke Details action
        var result = await controller.Details(1) as ViewResult;
        var model = result!.Model as UserDetailsViewModel;

        // Assert: Verify that the logs are being loaded correctly
        model!.Logs.Count.Should().Be(2);
        model.Logs[0].Action.Should().Be("Test1");

    }


    private User[] SetupUsers(string forename = "Johnny", string surname = "User", DateOnly dateOfBirth = default, string email = "juser@example.com", bool isActive = true)
    {

        if(dateOfBirth == default) {
            dateOfBirth = new DateOnly(2000, 1, 1);
        } 

        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                DateOfBirth = dateOfBirth,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();
    private UsersController CreateController() => new(_userService.Object, _logService.Object);
}
