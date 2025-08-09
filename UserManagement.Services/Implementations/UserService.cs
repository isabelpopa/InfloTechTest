using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Enums;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;

    private readonly IUserLogService _userLogService;

    private readonly IMemoryCache _cache;

    public UserService(IDataContext dataAccess, IUserLogService userLogService, IMemoryCache cache)
    {
        _dataAccess = dataAccess;
        _userLogService = userLogService;
        _cache = cache;
    }

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        var key = isActive ? UserCacheKey.ActiveUsers : UserCacheKey.InactiveUsers;
        return _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return _dataAccess.GetAll<User>()
                              .Where(u => u.IsActive == isActive)
                              .OrderBy(u => u.Id)
                              .ToList();
        })!;
    }

    public IEnumerable<User> GetAll() => _cache.GetOrCreate(UserCacheKey.AllUsers, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return _dataAccess.GetAll<User>().OrderBy(u => u.Id).ToList();
        })!;

    public async Task AddUserAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
        var userLog = new UserLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Description = user.Forename + " " + user.Surname + " was created",
            Action = "Created",
            DateTime = DateTime.Now
        };
        await _userLogService.AddUserLogAsync(userLog);
    }
    public async Task<User?> GetUserByIdAsync(long id)
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
        await _userLogService.AddUserLogAsync(userLog);
        return user;
    }
    public async Task EditUserAsync(User updatedUser)
    {
        await _dataAccess.UpdateAsync(updatedUser);

        var userLog = new UserLog
        {
            UserId = updatedUser.Id,
            Forename = updatedUser.Forename,
            Surname = updatedUser.Surname,
            Description = updatedUser.Forename + " " + updatedUser.Surname + " was edited",
            Action = "Edited",
            DateTime = DateTime.Now
        };
        await _userLogService.AddUserLogAsync(userLog);
    }
    public async Task DeleteUserAsync(User user)
    {
        await _dataAccess.DeleteAsync(user);

        var userLog = new UserLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Description = user.Forename + " " + user.Surname + " was deleted",
            Action = "Deleted",
            DateTime = DateTime.Now
        };
        await _userLogService.AddUserLogAsync(userLog);
    }
}
