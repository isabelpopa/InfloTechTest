using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;
public interface IUserLogService
{
    IEnumerable<UserLog> GetAll();
    Task AddUserLogAsync(UserLog userLog);
    UserLog? GetUserLogById(long Id);
    IEnumerable<UserLog> GetUserLogsByUserId(long id);
}
