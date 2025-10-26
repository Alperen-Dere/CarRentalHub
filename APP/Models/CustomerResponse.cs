namespace APP.Models;

public class CustomerResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    // Computed properties
    public string RegistrationDateFormatted => RegistrationDate.ToString("MMMM dd, yyyy");
}

