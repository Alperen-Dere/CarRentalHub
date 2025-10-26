using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Employee")]
public class CustomersController : Controller
{
    private readonly IService<CustomerRequest, CustomerResponse> _service;
    
    public CustomersController(IService<CustomerRequest, CustomerResponse> service)
    {
        _service = service;
    }
    
    // GET: Customers
    public IActionResult Index()
    {
        var customers = _service.GetAll();
        return View(customers);
    }
    
    // GET: Customers/Details/5
    public IActionResult Details(int id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
        {
            TempData["Error"] = "Customer not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(customer);
    }
    
    // GET: Customers/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Customers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CustomerRequest request)
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
    
    // GET: Customers/Edit/5
    public IActionResult Edit(int id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
        {
            TempData["Error"] = "Customer not found";
            return RedirectToAction(nameof(Index));
        }
        
        var request = new CustomerRequest
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address
        };
        
        return View(request);
    }
    
    // POST: Customers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CustomerRequest request)
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
    
    // GET: Customers/Delete/5
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
        {
            TempData["Error"] = "Customer not found";
            return RedirectToAction(nameof(Index));
        }
        
        return View(customer);
    }
    
    // POST: Customers/Delete/5
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

