using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations
{
    public class LogService : ILogService
    {
        private readonly List<UserLogEntry> _logs = new();
        public async Task AddAsync(UserLogEntry entry)
        {
            entry.Id = _logs.Count +1;
            entry.Timestamp = DateTime.UtcNow;
            _logs.Add(entry);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<UserLogEntry>> GetAllAsync()
        {
            return await Task.FromResult(_logs.AsEnumerable());
        }

        public async Task<IEnumerable<UserLogEntry>> GetByUserIdAsync(long userId)
        {
            var logs = _logs.Where(l => l.UserId == userId);
            return await Task.FromResult(logs);
        }
            

        public async Task <UserLogEntry?> GetByIdAsync(long id)
        {
            var log = _logs.FirstOrDefault(l => l.Id == id);
            return await Task.FromResult(log);
        }
            

    }
}