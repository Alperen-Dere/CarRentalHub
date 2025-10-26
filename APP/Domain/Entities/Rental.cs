using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Rental
{
    public int Id { get; set; }
    
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Range(0, 100000)]
    public decimal TotalCost { get; set; }
    
    public RentalStatus Status { get; set; } = RentalStatus.Pending;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public Payment? Payment { get; set; }
}

public enum RentalStatus
{
    Pending,
    Active,
    Completed,
    Cancelled
}

