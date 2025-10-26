using System.ComponentModel.DataAnnotations;

namespace APP.Models;

public class CarRequest
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Brand is required")]
    [StringLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
    public string Brand { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Model is required")]
    [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
    public string Model { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Year is required")]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Daily price is required")]
    [Range(0.01, 10000, ErrorMessage = "Daily price must be between $0.01 and $10,000")]
    [Display(Name = "Daily Price")]
    public decimal DailyPrice { get; set; }
    
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;
    
    [StringLength(50, ErrorMessage = "License plate cannot exceed 50 characters")]
    [Display(Name = "License Plate")]
    public string? LicensePlate { get; set; }
    
    [StringLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
    public string? Color { get; set; }
}

