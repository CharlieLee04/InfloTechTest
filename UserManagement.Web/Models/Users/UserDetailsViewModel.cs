using System;

namespace UserManagement.Web.Models.Users
{
    public class UserDetailsViewModel
    {
        public long Id { get; set; }
        public string Forename { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
