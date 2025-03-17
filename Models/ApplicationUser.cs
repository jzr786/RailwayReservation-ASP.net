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
        public bool IsActive { get; set; } = true; 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        public DateTime? LastLoginDate { get; set; } 

        public static string DefaultPassword => "Default@123"; 

        [Required] 
        [MaxLength(50)]
        public string RoleName { get; set; } = "Customer"; 
    }
}
