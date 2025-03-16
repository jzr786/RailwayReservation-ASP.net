using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservation.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true; // Default active

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default creation date

        public DateTime? LastLoginDate { get; set; } // Track last login date

        public static string DefaultPassword => "Default@123"; // Default alphanumeric password

        [Required] // ✅ Ensure RoleName is required
        [MaxLength(50)]
        public string RoleName { get; set; } = "Customer"; // ✅ Set a default role
    }
}
