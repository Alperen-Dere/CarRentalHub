using CORE.Results;
using Microsoft.EntityFrameworkCore;

namespace CORE.Services;

/// <summary>
/// Base service implementation providing common CRUD operations
/// </summary>
/// <typeparam name="TEntity">Database entity</typeparam>
/// <typeparam name="TRequest">Request DTO</typeparam>
/// <typeparam name="TResponse">Response DTO</typeparam>
public abstract class ServiceBase<TEntity, TRequest, TResponse> : IService<TRequest, TResponse>
    where TEntity : Entity, new() 
{
    protected readonly DbContext _context;
    
    protected ServiceBase(DbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Converts Request DTO to Entity
    /// Must be implemented by derived classes
    /// </summary>
    protected abstract TEntity ToEntity(TRequest request);
    
    /// <summary>
    /// Converts Entity to Response DTO
    /// Must be implemented by derived classes
    /// </summary>
    protected abstract TResponse ToResponse(TEntity entity);
    
    /// <summary>
    /// Gets the DbSet for the entity type
    /// </summary>
    protected virtual DbSet<TEntity> GetDbSet()
    {
        return _context.Set<TEntity>();
    }
    
    /// <summary>
    /// Creates a new entity
    /// </summary>
    public virtual CommandResult Create(TRequest request, bool save = true)
    {
        try
        {
            var entity = ToEntity(request);
            GetDbSet().Add(entity);
            if (save)
                Save();
            
            return CommandResult.Success("Record created successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error creating record: {ex.Message}");
        }
    }

    public virtual int Save() => 
        _context.SaveChanges();
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    public virtual CommandResult Update(TRequest request, bool save = true)
    {
        try
        {
            var entity = ToEntity(request);
            GetDbSet().Update(entity);
            if (save)
                Save();
            
            return CommandResult.Success("Record updated successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error updating record: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Deletes an entity by ID
    /// </summary>
    public virtual CommandResult Delete(int id, bool save = true)
    {
        try
        {
            var entity = GetDbSet().Find(id);
            if (entity == null)
            {
                return CommandResult.Failure("Record not found");
            }
            
            GetDbSet().Remove(entity);
            if (save)
                Save();
            
            return CommandResult.Success("Record deleted successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error deleting record: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Gets a single entity by ID
    /// </summary>
    public virtual TResponse? GetById(int id)
    {
        try
        {
            var entity = GetDbSet().Find(id);
            return entity == null ? default : ToResponse(entity);
        }
        catch
        {
            return default;
        }
    }
    
    /// <summary>
    /// Gets all entities
    /// </summary>
    public virtual List<TResponse> GetAll()
    {
        try
        {
            return GetDbSet()
                .AsNoTracking()
                .ToList()
                .Select(e => ToResponse(e))
                .ToList();
        }
        catch
        {
            return new List<TResponse>();
        }
    }
}

