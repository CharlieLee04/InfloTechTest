using System.Linq;
using System.Threading.Tasks;
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

    [HttpGet("create")]
    public IActionResult Create()
    {
        var model = new CreateUserViewModel();
        return View(model);
    }

    [HttpPost("create")]
    public IActionResult Create(CreateUserViewModel model)
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

        var result = _userService.Create(user);

        if(!result){
            ModelState.AddModelError(string.Empty, "An error occurred creating the user.");
            return View(model);
        }

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

        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.DateOfBirth = model.DateOfBirth;
        user.Email = model.Email; 
        user.IsActive = model.IsActive;

        var result = await _userService.Update(user);
        if(!result)
        {
            ModelState.AddModelError(string.Empty, "An error occured updating the users information");
            return View(model);
        }

        return RedirectToAction("List");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var result = await _userService.Delete(id);
        if(!result) return NotFound();
        return RedirectToAction("List");
    }

}
