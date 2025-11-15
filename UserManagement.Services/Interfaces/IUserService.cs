using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task <IEnumerable<User>> FilterByActive(bool isActive);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> CreateAsync(User user);
    Task<User?> GetByIdAsync(long id);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(long id);
    
}
