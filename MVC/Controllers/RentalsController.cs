using APP.Domain;
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Employee")]
public class RentalsController : Controller
{
    private readonly IService<RentalRequest, RentalResponse> _service;
    private readonly AppDbContext _context;
    
    public RentalsController(IService<RentalRequest, RentalResponse> service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }
    
    // GET: Rentals
    public IActionResult Index()
    {
        var rentals = _service.GetAll();
        return View(rentals);
    }
    
    // GET: Rentals/Details/5
    public IActionResult Details(int id)
    {
        var rental = _service.GetById(id);
        if (rental == null)
        {
            TempData["Error"] = "Rental not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(rental);
    }
    
    // GET: Rentals/Create
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }
    
    // POST: Rentals/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RentalRequest request)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(request);
        }
        
        var result = _service.Create(request);
        
        if (result.IsSuccess)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        
        TempData["Error"] = result.Message;
        PopulateDropdowns();
        return View(request);
    }
    
    // GET: Rentals/Edit/5
    public IActionResult Edit(int id)
    {
        var rental = _service.GetById(id);
        if (rental == null)
        {
            TempData["Error"] = "Rental not found";
            return RedirectToAction(nameof(Index));
        }
        
        var request = new RentalRequest
        {
            Id = rental.Id,
            CarId = rental.CarId,
            CustomerId = rental.CustomerId,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            Notes = rental.Notes
        };
        
        PopulateDropdowns();
        return View(request);
    }
    
    // POST: Rentals/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(RentalRequest request)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(request);
        }
        
        var result = _service.Update(request);
        
        if (result.IsSuccess)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        
        TempData["Error"] = result.Message;
        PopulateDropdowns();
        return View(request);
    }
    
    // GET: Rentals/Delete/5
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var rental = _service.GetById(id);
        if (rental == null)
        {
            TempData["Error"] = "Rental not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(rental);
    }
    
    // POST: Rentals/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        var result = _service.Delete(id);
        
        if (result.IsSuccess)
        {
            TempData["Success"] = result.Message;
        }
        else
        {
            TempData["Error"] = result.Message;
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    private void PopulateDropdowns()
    {
        ViewBag.Cars = new SelectList(
            _context.Cars.Where(c => c.IsAvailable).OrderBy(c => c.Brand).ThenBy(c => c.Model),
            "Id",
            "Brand",
            null,
            "Model"
        );
        
        ViewBag.Customers = new SelectList(
            _context.Customers.OrderBy(c => c.FullName),
            "Id",
            "FullName"
        );
    }
}

