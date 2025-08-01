using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;
public interface IUserLogService
{
    IEnumerable<UserLog> GetAll();
    void AddUserLog(UserLog userLog);
    UserLog? GetUserLogById(long Id);
    IEnumerable<UserLog> GetUserLogsByUserId(long id);
}
