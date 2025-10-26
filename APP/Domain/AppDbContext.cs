using APP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Domain;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) 
    {
    }
    
    // Users Module tables
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    // CarRental Domain tables
    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Payment> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure UserRole (many-to-many relationship)
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        
        // Configure Payment (one-to-one with Rental)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Rental)
            .WithOne(r => r.Payment)
            .HasForeignKey<Payment>(p => p.RentalId);
        
        // Seed data
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Employee" },
            new Role { Id = 3, Name = "Customer" }
        );
        
        // Seed Groups
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "Administrators", Description = "System administrators" },
            new Group { Id = 2, Name = "Staff", Description = "Company employees" },
            new Group { Id = 3, Name = "Customers", Description = "Regular customers" }
        );
        
        // Seed Users (password for all: password123)
        // Using a known valid BCrypt hash for "password123"  
        // Generated with: BCrypt.Net.BCrypt.HashPassword("password123", 11)
        // You can verify at: https://bcrypt-generator.com/
        const string passwordHash = "$2a$11$8K1p/a0dL2uzB9oB1YJpOuu9YgDqCjqwUH0ELQxaLnJQr6eQoK/oG";
        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@carrentalhub.com",
                PasswordHash = passwordHash,
                GroupId = 1
            },
            new User
            {
                Id = 2,
                Username = "employee1",
                Email = "employee@carrentalhub.com",
                PasswordHash = passwordHash,
                GroupId = 2
            },
            new User
            {
                Id = 3,
                Username = "customer1",
                Email = "customer@example.com",
                PasswordHash = passwordHash,
                GroupId = 3
            }
        );
        
        // Assign roles to users
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = 1, RoleId = 1 }, // admin -> Admin role
            new UserRole { UserId = 2, RoleId = 2 }, // employee1 -> Employee role
            new UserRole { UserId = 3, RoleId = 3 }  // customer1 -> Customer role
        );
        
        // Seed Cars
        modelBuilder.Entity<Car>().HasData(
            new Car
            {
                Id = 1,
                Brand = "Toyota",
                Model = "Camry",
                Year = 2023,
                DailyPrice = 50,
                IsAvailable = true,
                LicensePlate = "ABC123",
                Color = "Silver"
            },
            new Car
            {
                Id = 2,
                Brand = "Honda",
                Model = "Civic",
                Year = 2023,
                DailyPrice = 45,
                IsAvailable = true,
                LicensePlate = "XYZ789",
                Color = "Blue"
            },
            new Car
            {
                Id = 3,
                Brand = "BMW",
                Model = "X5",
                Year = 2024,
                DailyPrice = 120,
                IsAvailable = false, // Rented
                LicensePlate = "LUX001",
                Color = "Black"
            },
            new Car
            {
                Id = 4,
                Brand = "Tesla",
                Model = "Model 3",
                Year = 2024,
                DailyPrice = 100,
                IsAvailable = true,
                LicensePlate = "EV2024",
                Color = "White"
            },
            new Car
            {
                Id = 5,
                Brand = "Mercedes-Benz",
                Model = "C-Class",
                Year = 2023,
                DailyPrice = 110,
                IsAvailable = true,
                LicensePlate = "MER456",
                Color = "Gray"
            }
        );
        
        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "123 Main St, City, State 12345",
                RegistrationDate = new DateTime(2024, 1, 15)
            },
            new Customer
            {
                Id = 2,
                FullName = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "+1987654321",
                Address = "456 Oak Ave, Town, State 67890",
                RegistrationDate = new DateTime(2024, 2, 20)
            }
        );
        
        // Seed Rentals
        modelBuilder.Entity<Rental>().HasData(
            new Rental
            {
                Id = 1,
                CarId = 3, // BMW X5
                CustomerId = 1, // John Doe
                StartDate = new DateTime(2024, 10, 20),
                EndDate = new DateTime(2024, 10, 27),
                TotalCost = 840, // 7 days * $120
                Status = RentalStatus.Active,
                Notes = "Includes insurance coverage",
                CreatedAt = new DateTime(2024, 10, 19)
            }
        );
        
        // Seed Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = 1,
                RentalId = 1,
                Amount = 840,
                PaymentDate = new DateTime(2024, 10, 20),
                Status = PaymentStatus.Completed,
                PaymentMethod = "Credit Card",
                TransactionId = "TXN123456789"
            }
        );
    }
}

