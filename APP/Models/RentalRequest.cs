using System.ComponentModel.DataAnnotations;

namespace APP.Models;

public class RentalRequest
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Car is required")]
    [Display(Name = "Car")]
    public int CarId { get; set; }
    
    [Required(ErrorMessage = "Customer is required")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }
    
    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Today;
    
    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
    
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}

