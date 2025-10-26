using APP.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public class DebugController : Controller
{
    private readonly AppDbContext _context;
    
    public DebugController(AppDbContext context)
    {
        _context = context;
    }
    
    public IActionResult TestHash()
    {
        var password = "password123";
        
        // Generate a NEW hash
        var newHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        // Test verification with the new hash
        var verifyNew = BCrypt.Net.BCrypt.Verify(password, newHash);
        
        // Get the hash from database
        var user = _context.Users.FirstOrDefault(u => u.Username == "admin");
        var dbHash = user?.PasswordHash ?? "N/A";
        
        // Test verification with database hash
        var verifyDb = user != null ? BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) : false;
        
        var result = $@"
<h1>BCrypt Hash Test</h1>
<hr>
<h3>New Generated Hash:</h3>
<pre>{newHash}</pre>
<p><strong>Verification:</strong> {(verifyNew ? "✓ PASS" : "✗ FAIL")}</p>

<hr>
<h3>Database Hash (admin user):</h3>
<pre>{dbHash}</pre>
<p><strong>Verification:</strong> {(verifyDb ? "✓ PASS" : "✗ FAIL")}</p>

<hr>
<h3>Action:</h3>
{(verifyDb ? 
    "<p class='text-success'>✓ Database hash is VALID. Login should work!</p>" : 
    $@"<p class='text-danger'>✗ Database hash is INVALID. Need to update!</p>
    <form method='post' action='/Debug/UpdateHash'>
        <input type='hidden' name='newHash' value='{newHash}' />
        <button type='submit' class='btn btn-danger'>Update Database with Valid Hash</button>
    </form>"
)}
";
        
        return Content(result, "text/html");
    }
    
    [HttpPost]
    public IActionResult UpdateHash(string newHash)
    {
        try
        {
            // Update all users with the new valid hash
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                user.PasswordHash = newHash;
            }
            
            _context.SaveChanges();
            
            return Content($@"
<h1>✓ Success!</h1>
<p>All users updated with valid hash for password: <strong>password123</strong></p>
<p>New hash: <code>{newHash}</code></p>
<hr>
<a href='/Account/Login' class='btn btn-primary'>Go to Login</a>
<a href='/Debug/TestHash' class='btn btn-secondary'>Test Again</a>
", "text/html");
        }
        catch (Exception ex)
        {
            return Content($"Error: {ex.Message}", "text/html");
        }
    }
}

