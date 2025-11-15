using System.Collections.Generic;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Domain.Interfaces
{
    public interface ILogService
    {
        void add(UserLogEntry entry);
        IEnumerable<UserLogEntry> GetAll();
        IEnumerable<UserLogEntry> GetByUserId(long userId);
        UserLogEntry? GetById(long id);
    }
}
