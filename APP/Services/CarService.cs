using APP.Domain;
using APP.Domain.Entities;
using APP.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class CarService : ServiceBase<Car, CarRequest, CarResponse>
{
    private readonly AppDbContext _dbContext;
    
    public CarService(AppDbContext context) : base(context)
    {
        _dbContext = context;
    }
    
    protected override Car ToEntity(CarRequest request)
    {
        return new Car
        {
            Id = request.Id,
            Brand = request.Brand.Trim(),
            Model = request.Model.Trim(),
            Year = request.Year,
            DailyPrice = request.DailyPrice,
            IsAvailable = request.IsAvailable,
            LicensePlate = request.LicensePlate?.Trim(),
            Color = request.Color?.Trim()
        };
    }
    
    protected override CarResponse ToResponse(Car entity)
    {
        return new CarResponse
        {
            Id = entity.Id,
            Brand = entity.Brand,
            Model = entity.Model,
            Year = entity.Year,
            DailyPrice = entity.DailyPrice,
            IsAvailable = entity.IsAvailable,
            LicensePlate = entity.LicensePlate,
            Color = entity.Color
        };
    }
    
    /// <summary>
    /// Gets all available cars
    /// </summary>
    public List<CarResponse> GetAvailableCars()
    {
        return _dbContext.Cars
            .Where(c => c.IsAvailable)
            .AsNoTracking()
            .OrderBy(c => c.Brand)
            .ThenBy(c => c.Model)
            .Select(c => ToResponse(c))
            .ToList();
    }
}

