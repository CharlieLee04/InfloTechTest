using System;

namespace UserManagement.Web.Models.Logs
{
    public class LogEntryViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserForename { get; set; } = default!;
        public string UserSurname { get; set; } = default!;
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
