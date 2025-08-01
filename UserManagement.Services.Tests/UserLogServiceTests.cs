using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserLogServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userLogs = SetupUserLogs();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(userLogs);
    }

    [Fact]
    public void AddUserLog_PassedUserLog_VerifyDataContextCreateCalledOnce()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userLog = new UserLog()
        {
            Id = 1,
            UserId = 1,
            Forename = "Peter",
            Surname = "Loew",
            Description = "Peter Loew",
            Action = "Created",
            DateTime = new DateTime()
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddUserLog(userLog);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.Create(userLog), Times.Once);
    }

    [Fact]
    public void GetUserLogsByUserId_ValidUserLogId_ReturnsUserLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userLogs = SetupUserLogs();
        var userId = userLogs.First().UserId;
        var userLogsAttachedToUserId = userLogs.Where(s => s.UserId == userId);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetUserLogsByUserId(userId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(userLogsAttachedToUserId);
    }

    [Fact]
    public void GetUserLogById_ValidUserLogId_ReturnsUserLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userLogs = SetupUserLogs();
        var userLog = userLogs.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetUserLogById(userLog.Id);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(userLog);
    }

    [Fact]
    public void GetUserLogById_InvalidUserLogId_ReturnsNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userLogs = SetupUserLogs();
        var userLog = userLogs.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetUserLogById(userLog.Id - 2);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(null);
    }

    private IQueryable<UserLog> SetupUserLogs()
    {
        var userLogs = new List<UserLog>
        {
            new UserLog { Id = 1, UserId = 1, Forename = "Peter", Surname = "Loew", Description = "", Action = "", DateTime = new DateTime() },
            new UserLog { Id = 2, UserId = 2, Forename = "Benjamin Franklin", Surname = "Gates", Description = "", Action = "", DateTime = new DateTime() },
            new UserLog { Id = 3, UserId = 3, Forename = "Castor", Surname = "Troy", Description = "", Action = "", DateTime = new DateTime() },
            new UserLog { Id = 4, UserId = 4, Forename = "Memphis", Surname = "Raines", Description = "", Action = "", DateTime = new DateTime() },
            new UserLog { Id = 5, UserId = 5, Forename = "Stanley", Surname = "Goodspeed", Description = "", Action = "", DateTime = new DateTime() },
        }.AsQueryable();

        _dataContext
            .Setup(u => u.GetAll<UserLog>())
            .Returns(userLogs);

        return userLogs;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserLogService CreateService() => new(_dataContext.Object);
}
