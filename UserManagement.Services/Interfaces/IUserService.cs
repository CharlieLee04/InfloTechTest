using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state.
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task <IEnumerable<User>> FilterByActive(bool isActive);
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<User>> GetAllAsync();
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<bool> CreateAsync(User user);
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetByIdAsync(long id);
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(User user);
    /// <summary>
    /// Deletes a user by their unique identifier.
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(long id);
    
}
