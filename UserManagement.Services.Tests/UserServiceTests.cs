using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_PassedTrue_ReturnsActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.All(u => u.IsActive == true).Should().BeTrue();
    }

    [Fact]
    public void FilterByActive_PassedFalse_ReturnsInactiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.All(u => u.IsActive == false).Should().BeTrue();
    }

    [Fact]
    public async Task AddUser_PassedUser_VerifyDataContextCreateCalledOnce()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var user = new User()
        {
            Forename = "Rob",
            Surname = "Smith",
            Email = "test@gmail.com",
            DateOfBirth = new DateOnly (1999,05,19),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        await service.AddUserAsync(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.CreateAsync(user), Times.Once);
    }

    [Fact]
    public async Task GetUserById_ValidUserId_ReturnsUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetUserByIdAsync(user.Id);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(user);
    }

    [Fact]
    public async Task GetUserById_InvalidUserId_ReturnsNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetUserByIdAsync(user.Id - 2);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(null);
    }

    [Fact]
    public async Task EditUser_PassedUser_VerifyDataContextUpdateCalledOnce()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var user = new User()
        {
            Id = 1,
            Forename = "Rob",
            Surname = "Smith",
            Email = "test@gmail.com",
            DateOfBirth = new DateOnly(1999, 05, 19),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        await service.EditUserAsync(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_PassedUser_VerifyDataContextDeleteCalledOnce()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var user = new User()
        {
            Id = 1,
            Forename = "Peter",
            Surname = "Loew",
            Email = "ploew@example.com",
            DateOfBirth = new DateOnly(1980, 08, 12),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        await service.DeleteUserAsync(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.DeleteAsync(user), Times.Once);
    }

    private IQueryable<User> SetupUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", DateOfBirth = new DateOnly(1980, 08, 12), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", DateOfBirth = new DateOnly(2001, 02, 16), IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", DateOfBirth = new DateOnly(1988, 04, 04), IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", DateOfBirth = new DateOnly(2005, 12, 24), IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", DateOfBirth = new DateOnly(1975, 07, 21), IsActive = true },
        }.AsQueryable();

        _dataContext
            .Setup(u => u.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private readonly Mock<IUserLogService> _userLogService = new();
    private UserService CreateService() => new(_dataContext.Object, _userLogService.Object);
}
