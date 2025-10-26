using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Car
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;
    
    [Range(1900, 2100)]
    public int Year { get; set; }
    
    [Range(0, 10000)]
    public decimal DailyPrice { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    [StringLength(50)]
    public string? LicensePlate { get; set; }
    
    [StringLength(20)]
    public string? Color { get; set; }
    
    // Navigation properties
    public List<Rental> Rentals { get; set; } = new();
}

