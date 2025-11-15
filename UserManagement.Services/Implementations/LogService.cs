using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations
{
    public class LogService : ILogService
    {
        private readonly List<UserLogEntry> _logs = new();
        public void add(UserLogEntry entry)
        {
            entry.Id = _logs.Count +1;
            entry.Timestamp = DateTime.UtcNow;
            _logs.Add(entry);
        }

        public IEnumerable<UserLogEntry> GetAll() => _logs;

        public IEnumerable<UserLogEntry> GetByUserId(long userId)
            => _logs.Where(l => l.UserId == userId);

        public UserLogEntry? GetById(long id)
            => _logs.FirstOrDefault(l => l.Id == id);

    }
}