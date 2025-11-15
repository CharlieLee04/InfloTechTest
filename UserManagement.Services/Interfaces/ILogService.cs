using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Domain.Interfaces
{
    public interface ILogService
    {
        Task AddAsync(UserLogEntry entry);
        Task<IEnumerable<UserLogEntry>> GetAllAsync();
        Task<IEnumerable<UserLogEntry>> GetByUserIdAsync(long userId);
        Task<UserLogEntry?> GetByIdAsync(long id);
    }
}
