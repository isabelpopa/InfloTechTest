using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;
public class UserLogService : IUserLogService
{
    private readonly IDataContext _dataAccess;
    public UserLogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<UserLog> GetAll() => _dataAccess.GetAll<UserLog>();
    public void AddUserLog(UserLog userLog) => _dataAccess.Create(userLog);
    public UserLog? GetUserLogById(long id) => _dataAccess.GetAll<UserLog>().SingleOrDefault(l => l.Id == id);
    public IEnumerable<UserLog> GetUserLogsByUserId(long id) => _dataAccess.GetAll<UserLog>().Where(s => s.UserId == id);
}
