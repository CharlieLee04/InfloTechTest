using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List("active");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_FilterActive_ShouldReturnOnlyActiveUsers()
    {
        // Arrange: Create controller and mock users 
        var controller = CreateController();
        var users = new[]
        {
            new User { Id = 1, Forename = "ActiveUser", IsActive = true },
            new User { Id = 2, Forename = "Inactive User", IsActive = false }
        };

        _userService.Setup(s => s.GetAll()).Returns(users);

        // Act: Call List with filter active
        var result = controller.List("active");

        // Assert: Only the active user is included 
        var model = result.Model.Should().BeOfType<UserListViewModel>().Subject;
        model.Items.Should().OnlyContain(u => u.IsActive).And.HaveCount(1);
    }

    [Fact]
    public void CreatePost_ValidModel_ShouldRedirectToList()
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

        _userService.Setup(s => s.Create(It.IsAny<User>())).Returns(true);

        // Act: Invokes the Create action with the arranged model.
        var result = controller.Create(model);

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
        _userService.Setup(s => s.Update(It.IsAny<User>())).ReturnsAsync(true);

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
        _userService.Setup(s => s.Delete(1)).ReturnsAsync(true);

        // Act: Invoke the DeleteUser POST action using the user ID
        var result = await controller.DeleteUser(1);

        // Assert: Verify that the action redirtects to the list page after successful deletion.
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
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
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
