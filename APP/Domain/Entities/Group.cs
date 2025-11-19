using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Group : Entity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    // Navigation properties
    public List<User> Users { get; set; } = new();
}

