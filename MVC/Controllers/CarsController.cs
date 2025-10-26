using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Authorize] // All authenticated users can view cars
public class CarsController : Controller
{
    private readonly IService<CarRequest, CarResponse> _service;
    
    public CarsController(IService<CarRequest, CarResponse> service)
    {
        _service = service;
    }
    
    // GET: Cars
    public IActionResult Index()
    {
        var cars = _service.GetAll();
        return View(cars);
    }
    
    // GET: Cars/Details/5
    public IActionResult Details(int id)
    {
        var car = _service.GetById(id);
        if (car == null)
        {
            TempData["Error"] = "Car not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(car);
    }
    
    // GET: Cars/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Cars/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(CarRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        
        var result = _service.Create(request);
        
        if (result.IsSuccess)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        
        TempData["Error"] = result.Message;
        return View(request);
    }
    
    // GET: Cars/Edit/5
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var car = _service.GetById(id);
        if (car == null)
        {
            TempData["Error"] = "Car not found";
            return RedirectToAction(nameof(Index));
        }
        
        // Convert Response to Request for editing
        var request = new CarRequest
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            DailyPrice = car.DailyPrice,
            IsAvailable = car.IsAvailable,
            LicensePlate = car.LicensePlate,
            Color = car.Color
        };
        
        return View(request);
    }
    
    // POST: Cars/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(CarRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        
        var result = _service.Update(request);
        
        if (result.IsSuccess)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        
        TempData["Error"] = result.Message;
        return View(request);
    }
    
    // GET: Cars/Delete/5
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var car = _service.GetById(id);
        if (car == null)
        {
            TempData["Error"] = "Car not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(car);
    }
    
    // POST: Cars/Delete/5
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
}

