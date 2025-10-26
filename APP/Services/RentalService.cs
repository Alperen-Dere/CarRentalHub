using APP.Domain;
using APP.Domain.Entities;
using APP.Models;
using CORE.Results;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class RentalService : ServiceBase<Rental, RentalRequest, RentalResponse>
{
    private readonly AppDbContext _dbContext;
    
    public RentalService(AppDbContext context) : base(context)
    {
        _dbContext = context;
    }
    
    protected override Rental ToEntity(RentalRequest request)
    {
        var rental = _dbContext.Rentals.Find(request.Id);
        
        if (rental == null)
        {
            // Creating new rental - calculate cost
            var car = _dbContext.Cars.Find(request.CarId);
            var days = (request.EndDate - request.StartDate).Days;
            var totalCost = car != null ? days * car.DailyPrice : 0;
            
            return new Rental
            {
                CarId = request.CarId,
                CustomerId = request.CustomerId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalCost = totalCost,
                Notes = request.Notes,
                Status = RentalStatus.Pending,
                CreatedAt = DateTime.Now
            };
        }
        else
        {
            // Updating existing rental
            rental.CarId = request.CarId;
            rental.CustomerId = request.CustomerId;
            rental.StartDate = request.StartDate;
            rental.EndDate = request.EndDate;
            rental.Notes = request.Notes;
            
            // Recalculate cost
            var car = _dbContext.Cars.Find(request.CarId);
            var days = (request.EndDate - request.StartDate).Days;
            rental.TotalCost = car != null ? days * car.DailyPrice : 0;
            
            return rental;
        }
    }
    
    protected override RentalResponse ToResponse(Rental entity)
    {
        return new RentalResponse
        {
            Id = entity.Id,
            CarId = entity.CarId,
            CarName = entity.Car != null ? $"{entity.Car.Brand} {entity.Car.Model} ({entity.Car.Year})" : "N/A",
            CustomerId = entity.CustomerId,
            CustomerName = entity.Customer?.FullName ?? "N/A",
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            TotalCost = entity.TotalCost,
            Status = entity.Status.ToString(),
            Notes = entity.Notes,
            CreatedAt = entity.CreatedAt
        };
    }
    
    public override List<RentalResponse> GetAll()
    {
        return _dbContext.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => ToResponse(r))
            .ToList();
    }
    
    public override RentalResponse? GetById(int id)
    {
        var rental = _dbContext.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == id);
        
        return rental == null ? null : ToResponse(rental);
    }
    
    public override CommandResult Create(RentalRequest request)
    {
        try
        {
            // Validation: Check if car exists
            var car = _dbContext.Cars.Find(request.CarId);
            if (car == null)
                return CommandResult.Failure("Car not found");
            
            // Validation: Check if car is available
            if (!car.IsAvailable)
                return CommandResult.Failure("Car is not available for rental");
            
            // Validation: Check date range
            if (request.EndDate <= request.StartDate)
                return CommandResult.Failure("End date must be after start date");
            
            if (request.StartDate < DateTime.Today)
                return CommandResult.Failure("Start date cannot be in the past");
            
            // Validation: Check if customer exists
            var customer = _dbContext.Customers.Find(request.CustomerId);
            if (customer == null)
                return CommandResult.Failure("Customer not found");
            
            // Create rental
            var entity = ToEntity(request);
            entity.Status = RentalStatus.Active;
            
            _dbContext.Rentals.Add(entity);
            
            // Mark car as unavailable
            car.IsAvailable = false;
            
            _dbContext.SaveChanges();
            
            return CommandResult.Success($"Rental created successfully. Total cost: ${entity.TotalCost:F2}");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error creating rental: {ex.Message}");
        }
    }
    
    public override CommandResult Delete(int id)
    {
        try
        {
            var rental = _dbContext.Rentals.Find(id);
            if (rental == null)
                return CommandResult.Failure("Rental not found");
            
            // Make car available again
            var car = _dbContext.Cars.Find(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
            }
            
            _dbContext.Rentals.Remove(rental);
            _dbContext.SaveChanges();
            
            return CommandResult.Success("Rental deleted successfully. Car is now available.");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error deleting rental: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Completes a rental and makes the car available again
    /// </summary>
    public CommandResult CompleteRental(int rentalId)
    {
        try
        {
            var rental = _dbContext.Rentals.Find(rentalId);
            if (rental == null)
                return CommandResult.Failure("Rental not found");
            
            rental.Status = RentalStatus.Completed;
            
            // Make car available again
            var car = _dbContext.Cars.Find(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
            }
            
            _dbContext.SaveChanges();
            
            return CommandResult.Success("Rental completed successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error completing rental: {ex.Message}");
        }
    }
}

