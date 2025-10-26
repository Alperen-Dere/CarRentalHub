using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public List<UserRole> UserRoles { get; set; } = new();
}

