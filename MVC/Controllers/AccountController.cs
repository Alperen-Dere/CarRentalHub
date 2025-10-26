using APP.Domain;
using APP.Models;
using CORE.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public class AccountController : Controller
{
    private readonly ICookieAuthService _authService;
    private readonly AppDbContext _context;
    
    public AccountController(ICookieAuthService authService, AppDbContext context)
    {
        _authService = authService;
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        if (_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            // Find user
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            
            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            
            // Get user's role (first role if multiple)
            var role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "Customer";
            
            // Sign in
            await _authService.SignInAsync(user.Username, role, user.Email);
            
            TempData["Success"] = $"Welcome back, {user.Username}!";
            
            // Redirect based on role
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Cars");
            }
            else if (role == "Employee")
            {
                return RedirectToAction("Index", "Rentals");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Login failed: {ex.Message}");
            return View(model);
        }
    }
    
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();
        TempData["Info"] = "You have been logged out successfully";
        return RedirectToAction("Login");
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        if (_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return View(model);
            }
            
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered");
                return View(model);
            }
            
            // Get Customer group (default for new registrations)
            var customerGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == "Customers");
            if (customerGroup == null)
            {
                ModelState.AddModelError("", "Registration system error: Customer group not found");
                return View(model);
            }
            
            // Get Customer role
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (customerRole == null)
            {
                ModelState.AddModelError("", "Registration system error: Customer role not found");
                return View(model);
            }
            
            // Create new user with hashed password
            var newUser = new APP.Domain.Entities.User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                GroupId = customerGroup.Id
            };
            
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            
            // Assign Customer role
            var userRole = new APP.Domain.Entities.UserRole
            {
                UserId = newUser.Id,
                RoleId = customerRole.Id
            };
            
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Registration successful! Please login with your credentials.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Registration failed: {ex.Message}");
            return View(model);
        }
    }
}

