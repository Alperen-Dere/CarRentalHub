using System.ComponentModel.DataAnnotations;

namespace APP.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    
    public int RentalId { get; set; }
    public Rental Rental { get; set; } = null!;
    
    [Range(0, 100000)]
    public decimal Amount { get; set; }
    
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    [StringLength(50)]
    public string? PaymentMethod { get; set; }
    
    [StringLength(100)]
    public string? TransactionId { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

