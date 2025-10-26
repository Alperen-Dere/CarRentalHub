using APP.Domain;
using APP.Domain.Entities;
using APP.Models;
using CORE.Results;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public class CustomerService : ServiceBase<Customer, CustomerRequest, CustomerResponse>
{
    private readonly AppDbContext _dbContext;
    
    public CustomerService(AppDbContext context) : base(context)
    {
        _dbContext = context;
    }
    
    protected override Customer ToEntity(CustomerRequest request)
    {
        var customer = _dbContext.Customers.Find(request.Id);
        
        if (customer == null)
        {
            // Creating new customer
            return new Customer
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone?.Trim(),
                Address = request.Address?.Trim(),
                RegistrationDate = DateTime.Now
            };
        }
        else
        {
            // Updating existing customer
            customer.FullName = request.FullName.Trim();
            customer.Email = request.Email.Trim();
            customer.Phone = request.Phone?.Trim();
            customer.Address = request.Address?.Trim();
            return customer;
        }
    }
    
    protected override CustomerResponse ToResponse(Customer entity)
    {
        return new CustomerResponse
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            Phone = entity.Phone,
            Address = entity.Address,
            RegistrationDate = entity.RegistrationDate
        };
    }
    
    public override CommandResult Create(CustomerRequest request)
    {
        try
        {
            // Check for duplicate email
            if (_dbContext.Customers.Any(c => c.Email == request.Email))
            {
                return CommandResult.Failure("A customer with this email already exists");
            }
            
            return base.Create(request);
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error creating customer: {ex.Message}");
        }
    }
}

