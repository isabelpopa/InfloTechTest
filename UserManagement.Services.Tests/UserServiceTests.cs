using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

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
    public void AddUser_PassedUser_VerifyDataContextCreateCalledOnce()
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
        service.AddUser(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.Create(user), Times.Once);
    }

    [Fact]
    public void GetUserById_ValidUserId_ReturnsUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetUserById(user.Id);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(user);
    }

    [Fact]
    public void GetUserById_InvalidUserId_ReturnsNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetUserById(user.Id - 2);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Be(null);
    }

    [Fact]
    public void EditUser_PassedUser_VerifyDataContextUpdateCalledOnce()
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
        service.EditUser(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.Update(user), Times.Once);
    }

    [Fact]
    public void DeleteUser_PassedUser_VerifyDataContextDeleteCalledOnce()
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
        service.DeleteUser(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(u => u.Delete(user), Times.Once);
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
    private UserService CreateService() => new(_dataContext.Object);
}
