using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(string filter)
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
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
}
