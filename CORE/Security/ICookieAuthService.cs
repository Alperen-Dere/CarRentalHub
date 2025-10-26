namespace CORE.Security;

/// <summary>
/// Service for cookie-based authentication
/// </summary>
public interface ICookieAuthService
{
    /// <summary>
    /// Signs in a user with cookie authentication
    /// </summary>
    Task SignInAsync(string username, string role, string? email = null);
    
    /// <summary>
    /// Signs out the current user
    /// </summary>
    Task SignOutAsync();
    
    /// <summary>
    /// Gets the current user's username
    /// </summary>
    string? GetCurrentUsername();
    
    /// <summary>
    /// Checks if a user is authenticated
    /// </summary>
    bool IsAuthenticated();
}

