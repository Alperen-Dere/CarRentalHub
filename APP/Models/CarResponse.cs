namespace APP.Models;

public class CarResponse
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal DailyPrice { get; set; }
    public bool IsAvailable { get; set; }
    public string? LicensePlate { get; set; }
    public string? Color { get; set; }
    
    // Computed properties for display
    public string DisplayName => $"{Brand} {Model} ({Year})";
    public string AvailabilityStatus => IsAvailable ? "Available" : "Rented";
    public string PriceFormatted => $"${DailyPrice:F2}/day";
}

