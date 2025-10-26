using CORE.Results;

namespace CORE.Services;

/// <summary>
/// Generic service interface for CRUD operations
/// </summary>
/// <typeparam name="TRequest">Request DTO (input with validation)</typeparam>
/// <typeparam name="TResponse">Response DTO (output for display)</typeparam>
public interface IService<TRequest, TResponse>
{
    /// <summary>
    /// Creates a new entity
    /// </summary>
    CommandResult Create(TRequest request);
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    CommandResult Update(TRequest request);
    
    /// <summary>
    /// Deletes an entity by ID
    /// </summary>
    CommandResult Delete(int id);
    
    /// <summary>
    /// Gets a single entity by ID
    /// </summary>
    TResponse? GetById(int id);
    
    /// <summary>
    /// Gets all entities
    /// </summary>
    List<TResponse> GetAll();
}

