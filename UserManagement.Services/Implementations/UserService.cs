using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;

    private readonly IUserLogService _userLogService;

    public UserService(IDataContext dataAccess, IUserLogService userLogService)
    {
        _dataAccess = dataAccess;
        _userLogService = userLogService;
    } 

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public void AddUser(User user)
    {
        _dataAccess.Create(user);
        var userLog = new UserLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Description = user.Forename + " " + user.Surname + " was created",
            Action = "Created",
            DateTime = DateTime.Now
        };
        _userLogService.AddUserLog(userLog);
    }
    public User? GetUserById(long id)
    {
        var user = _dataAccess.GetAll<User>().SingleOrDefault(u => u.Id == id);
        if (user == null)
            return null;
        var userLog = new UserLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Description = user.Forename + " " + user.Surname + " was viewed",
            Action = "Viewed",
            DateTime = DateTime.Now
        };
        _userLogService.AddUserLog(userLog);
        return user;
    }
    public void EditUser(User updatedUser)
    {
        _dataAccess.Update(updatedUser);

        var userLog = new UserLog
        {
            UserId = updatedUser.Id,
            Forename = updatedUser.Forename,
            Surname = updatedUser.Surname,
            Description = updatedUser.Forename + " " + updatedUser.Surname + " was edited",
            Action = "Edited",
            DateTime = DateTime.Now
        };
        _userLogService.AddUserLog(userLog);
    }
    public void DeleteUser(User user)
    {
        _dataAccess.Delete(user);

        var userLog = new UserLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Description = user.Forename + " " + user.Surname + " was deleted",
            Action = "Deleted",
            DateTime = DateTime.Now
        };
        _userLogService.AddUserLog(userLog);
    }
}
