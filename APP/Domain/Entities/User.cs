using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    
    // Navigation properties
    public List<UserRole> UserRoles { get; set; } = new();
}

