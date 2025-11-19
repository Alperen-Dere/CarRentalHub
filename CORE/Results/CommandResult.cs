namespace CORE.Results;

/// <summary>
/// Represents the result of a command operation (Create, Update, Delete)
/// </summary>
public class CommandResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; } // should be generic type, not object
    
    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static CommandResult Success(string? message = "Operation completed successfully")
    {
        return new CommandResult
        {
            IsSuccess = true,
            Message = message
        };
    }
    
    /// <summary>
    /// Creates a successful result with data
    /// </summary>
    public static CommandResult Success(string message, object data)
    {
        return new CommandResult
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static CommandResult Failure(string message)
    {
        return new CommandResult
        {
            IsSuccess = false,
            Message = message
        };
    }
}

