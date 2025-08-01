using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.UserLogs;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    private readonly IUserLogService _userLogService;
    public UsersController(IUserService userService, IUserLogService userLogService)
    {
        _userService = userService;
        _userLogService = userLogService;
    }

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        IEnumerable<User> users;

        if (isActive.HasValue)
        {
            users = _userService.FilterByActive(isActive.Value);
        }
        else
        {
            users = _userService.GetAll();
        }

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.OrderBy(u => u.Id).ToList()
        };

        return View(model);
    }

    [HttpGet("add")]
    public ViewResult Add() => View();

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateUserModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User()
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            IsActive = model.IsActive
        };

        await _userService.AddUserAsync(user);

        return RedirectToAction("List");
    }

    [HttpGet("view/{id}")]
    public async Task<IActionResult> View(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        var userLogs = _userLogService.GetUserLogsByUserId(id);

        var userLogItems = userLogs.Select(u => new UserLogListItemViewModel
        {
            Id = u.Id,
            UserId = u.UserId,
            Forename = u.Forename,
            Surname = u.Surname,
            Action = u.Action,
            DateTime = u.DateTime
        });

        var userId = new UserDetailsViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            UserLogs = userLogItems.ToList()
        };
        return View(userId);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Update(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        var userEditModel = new UpdateUserModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        return View("Edit", userEditModel);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> ChangedUser(UpdateUserModel model)
    {
        if (!ModelState.IsValid)
            return View("Edit", model);

        var updatedUser = new User
        {
            Id = model.Id,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            IsActive = model.IsActive
        };

        await _userService.EditUserAsync(updatedUser);

        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        var userViewModel = new UserDetailsViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        return View("Delete", userViewModel);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userService.DeleteUserAsync(user);

        return RedirectToAction("List");
    }

};
