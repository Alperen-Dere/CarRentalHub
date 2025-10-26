using APP.Domain;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class TestController : Controller
{
    private readonly AppDbContext _context;
    
    public TestController(AppDbContext context)
    {
        _context = context;
    }
    
    // Navigate to: https://localhost:5001/Test/Database
    public IActionResult Database()
    {
        var cars = _context.Cars.ToList();
        
        var message = "✅ DATABASE CONNECTED SUCCESSFULLY!\n\n";
        message += $"Database Location: MVC/CarRentalHub.db\n";
        message += $"Found {cars.Count} cars in database:\n\n";
        
        foreach (var car in cars)
        {
            var availability = car.IsAvailable ? "✓ Available" : "✗ Rented";
            message += $"  • {car.Brand} {car.Model} ({car.Year})\n";
            message += $"    Price: ${car.DailyPrice}/day | Status: {availability}\n\n";
        }
        
        message += "\n🎉 Your database is working perfectly!\n";
        message += "Next: Follow QUICK_START.md to implement the full architecture.";
        
        return Content(message, "text/plain");
    }
}

