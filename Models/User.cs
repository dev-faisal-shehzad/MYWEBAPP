using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyWebApp.Models
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }

    public enum UserStatus
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
        Blocked = 3
    }

    public enum UserGender
    {
        Male = 0,
        Female = 1,
        Other = 3
    }

    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Role))] 
    [Index(nameof(Status))]
    [Index(nameof(Gender))]
    [Index(nameof(IsDeleted))]
    [Index(nameof(EmailConfirmed))]
    public class User : IdentityUser<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 60 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 60 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public override string Email { get; set; } = string.Empty;

        [Required]
        public override bool EmailConfirmed { get; set; } = false;

        [Required]
        public UserStatus Status { get; set; } = UserStatus.Pending;   

        [Required]
        public UserGender Gender { get; set; } = UserGender.Male;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Manually updated

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; } // Nullable DateTime

        // ✅ One-to-Many Relationship: One User has Many Posts
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
