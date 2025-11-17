using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Domain.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Adds a new user log entry to the system.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task AddAsync(UserLogEntry entry);
        /// <summary>
        /// Retrieves all user log entries.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserLogEntry>> GetAllAsync();
        /// <summary>
        /// Retrieves all log entries associated with a specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserLogEntry>> GetByUserIdAsync(long userId);
        /// <summary>
        /// Retrieves a specific log entry by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserLogEntry?> GetByIdAsync(long id);
    }
}
