using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActive(bool isActive)
    {
        return await Task.FromResult(_dataAccess.GetAll<User>().Where(u => u.IsActive == isActive));
    }

    public async Task <IEnumerable<User>> GetAllAsync()
    {
        return await Task.FromResult(_dataAccess.GetAll<User>());
    }

    public async Task<bool> CreateAsync(User user)
    {
        try
        {
            await _dataAccess.CreateAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
        
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _dataAccess.GetByIdAsync<User>(id);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        try
        {
            await _dataAccess.UpdateAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _dataAccess.GetByIdAsync<User>(id);
        if(user == null) return false;
        
        await _dataAccess.DeleteAsync(user);
        return true;

    }
}
