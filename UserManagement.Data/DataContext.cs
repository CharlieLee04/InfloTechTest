using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", DateOfBirth = new (1989, 11, 23), Email = "ploew@example.com", IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", DateOfBirth = new (1989, 11, 23), Email = "bfgates@example.com", IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", DateOfBirth = new (1989, 11, 23), Email = "ctroy@example.com", IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", DateOfBirth = new (1989, 11, 23), Email = "mraines@example.com", IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", DateOfBirth = new (1989, 11, 23), Email = "sgodspeed@example.com", IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", DateOfBirth = new (1989, 11, 23), Email = "himcdunnough@example.com", IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", DateOfBirth = new (1989, 11, 23), Email = "cpoe@example.com", IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", DateOfBirth = new (1989, 11, 23), Email = "emalus@example.com", IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", DateOfBirth = new (1989, 11, 23), Email = "dmacready@example.com", IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", DateOfBirth = new (1989, 11, 23), Email = "jblaze@example.com", IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", DateOfBirth = new (1989, 11, 23), Email = "rfeld@example.com", IsActive = true },
        });

    public DbSet<User>? Users { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(long id) where TEntity : class
    {
        return await base.Set<TEntity>().FindAsync(id);
    }

    //public async Task SaveChangesAsync() =>
       //await base.SaveChangesAsync();
}
