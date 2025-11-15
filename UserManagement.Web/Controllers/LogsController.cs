using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(string search ="", string sortOrder = "asc")
{
    var allLogs = await _logService.GetAllAsync();
    var logs = allLogs.Select(l => new LogEntryViewModel
    {
        Id = l.Id,
        UserId = l.UserId,
        UserForename = l.UserForename,
        UserSurname = l.UserSurname,
        Action = l.Action,
        Details = l.Details,
        Timestamp = l.Timestamp
    });

    // Filter by search
    if(!string.IsNullOrEmpty(search))
    {
        logs = logs.Where(l => 
            (l.UserForename + " " + l.UserSurname)
            .Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    // Sort by Id ascending or descening
    logs = sortOrder == "desc" ? logs.OrderByDescending(l => l.Id) : logs.OrderBy(l => l.Id);

    return View(logs.ToList()); 
}

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var log = await _logService.GetByIdAsync(id);
        if(log == null) return NotFound();

        var model = new LogEntryViewModel
        {
            Id = log.Id,
            UserId = log.UserId,
            UserForename = log.UserForename,
            UserSurname = log.UserSurname,
            Action = log.Action,
            Details = log.Details,
            Timestamp = log.Timestamp
        };

        return View(model);
    }
    
}