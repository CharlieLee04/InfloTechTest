using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users
{
    public class EditUserViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Forename { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}