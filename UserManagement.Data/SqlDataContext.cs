using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class SqlDataContext : DbContext, IDataContext
{
    public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", DateOfBirth = new DateOnly(1980, 08, 12), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", DateOfBirth = new DateOnly(2001, 02, 16), IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", DateOfBirth = new DateOnly(1988, 04, 04), IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", DateOfBirth = new DateOnly(2005, 12, 24), IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", DateOfBirth = new DateOnly(1975, 07, 21), IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", DateOfBirth = new DateOnly(1999, 06, 14), IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", DateOfBirth = new DateOnly(2002, 11, 23), IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", DateOfBirth = new DateOnly(1997, 09, 11), IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", DateOfBirth = new DateOnly(1998, 05, 04), IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", DateOfBirth = new DateOnly(2000, 02, 18), IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", DateOfBirth = new DateOnly(2004, 10, 12), IsActive = true },
        });

    public DbSet<User>? Users { get; set; }
    public DbSet<UserLog>? UserLogs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => Set<TEntity>();

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Add(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Remove(entity);
        await SaveChangesAsync();
    }
}
