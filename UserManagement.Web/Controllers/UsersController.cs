using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogService _logService;
    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    [HttpGet]
    public async Task<ViewResult> List(string filter)
    {
        var allUsers = await _userService.GetAllAsync();

        var items = allUsers.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Email = p.Email,
            IsActive = p.IsActive
        });

        switch(filter){
            case "active":
            items = items.Where(u => u.IsActive);
            break;

            case "inactive":
            items = items.Where(u => !u.IsActive);
            break; 

            // default: select all users
        } 

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create() => View(new CreateUserViewModel());
  
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new Models.User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            DateOfBirth = model.DateOfBirth,
            Email = model.Email,
            IsActive = model.IsActive
        };

        var result = await _userService.CreateAsync(user);

        if(!result){
            ModelState.AddModelError(string.Empty, "An error occurred creating the user.");
            return View(model);
        }

        // Create Logs for user creation

        _logService.add(new UserLogEntry
        {
            UserId = user.Id,
            UserForename = user.Forename,
            UserSurname = user.Surname,
            Action = "User Created",
            Timestamp = DateTime.UtcNow
        });

        return RedirectToAction("List");
        
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        if(user == null) return NotFound();

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(long id, EditUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userService.GetByIdAsync(id);
        if(user == null) return NotFound();

        // Collect changes 
        var changes = new List<string>();

        if(user.Forename != model.Forename)
            changes.Add($"Forename changed from '{user.Forename}' to '{model.Forename}'");

        if(user.Surname != model.Surname)
            changes.Add($"Surname changed from '{user.Surname}' to '{model.Surname}'");

        if(user.DateOfBirth != model.DateOfBirth)
            changes.Add($"Date Of Birth changed from '{user.DateOfBirth}' to '{model.DateOfBirth}'");

        if(user.Email != model.Email)
            changes.Add($"Email changed from '{user.Email}' to '{model.Email}'");

        if(user.IsActive != model.IsActive)
            changes.Add($"IsActive changed from '{user.IsActive}' to '{model.IsActive}'");

        // Apply Updates
        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.DateOfBirth = model.DateOfBirth;
        user.Email = model.Email; 
        user.IsActive = model.IsActive;

        var result = await _userService.UpdateAsync(user);
        if(!result)
        {
            ModelState.AddModelError(string.Empty, "An error occured updating the users information");
            return View(model);
        }

        // Create a log if changes were made 
        if (changes.Any())
        {
            _logService.add(new UserLogEntry
            {
                UserId = user.Id,
                UserForename = user.Forename,
                UserSurname = user.Surname,
                Action = "User Edited",
                Details = string.Join("; ", changes),
                Timestamp = DateTime.UtcNow
            });
        }
        
        return RedirectToAction("List");
    }
    
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        if(user == null) return NotFound();

         var logs = _logService.GetByUserId(id);

        var model = new UserDetailsViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive,
            Logs = logs.ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        if(user == null) return NotFound();

        var result = await _userService.DeleteAsync(id);
        if(!result) return NotFound();

        // Add logging for deleting users
        _logService.add(new UserLogEntry
        {
            UserId = id,
            UserForename = user.Forename,
            UserSurname = user.Surname,
            Action = "User Deleted",
            Timestamp = DateTime.UtcNow
        });

        return RedirectToAction("List");
    }

}
