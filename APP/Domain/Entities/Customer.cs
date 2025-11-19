using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Customer : Entity
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }
    
    [StringLength(200)]
    public string? Address { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    
    // Navigation properties
    public List<Rental> Rentals { get; set; } = new();
}

