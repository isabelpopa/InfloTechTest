using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.UserLogs;

namespace UserManagement.Web.Controllers;
public class UserLogsController : Controller
{
    private readonly IUserLogService _userLogService;

    public UserLogsController(IUserLogService userLogService)
    {
        _userLogService = userLogService;
    }


    [HttpGet]
    public IActionResult List(bool descending = true)
    {
        var userLogs = _userLogService.GetAll();

        var items = userLogs.Select(l => new UserLogListItemViewModel
        {
            Id = l.Id,
            UserId = l.UserId,
            Forename = l.Forename,
            Surname = l.Surname,
            Action = l.Action,
            DateTime = l.DateTime
        });

        if (descending == true)
        {
            items = items.OrderByDescending(l => l.DateTime);
        }

        var model = new UserLogListViewModel
        {
            Items = items.ToList()
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult View(long id)
    {
        var userLog = _userLogService.GetUserLogById(id);
        if (userLog == null)
            return NotFound();

        var model = new UserLogDetailsViewModel
        {
            Id = userLog.Id,
            UserId = userLog.UserId,
            Forename = userLog.Forename,
            Surname = userLog.Surname,
            Action = userLog.Action,
            Description = userLog.Description,
            DateTime = userLog.DateTime
        };

        return View(model);
    }
}
