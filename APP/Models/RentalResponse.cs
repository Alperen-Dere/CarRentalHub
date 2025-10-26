namespace APP.Models;

public class RentalResponse
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Computed properties
    public int Duration => (EndDate - StartDate).Days;
    public string DurationText => $"{Duration} day{(Duration != 1 ? "s" : "")}";
    public string TotalCostFormatted => $"${TotalCost:F2}";
    public string StartDateFormatted => StartDate.ToString("MM/dd/yyyy");
    public string EndDateFormatted => EndDate.ToString("MM/dd/yyyy");
}

