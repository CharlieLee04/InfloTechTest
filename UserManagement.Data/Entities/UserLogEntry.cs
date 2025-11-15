using System;

namespace UserManagement.Data.Entities
{
    public class UserLogEntry
    {
        public long Id { get; set; }
        public long UserId { get; set;}
        public string UserForename { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string Action { get; set;} = default!;
        public string Details { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}