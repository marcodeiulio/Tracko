using APIs.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace APIs.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Roles> Roles => Set<Roles>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<JobApplicationStatuses> JobApplicationStatuses => Set<JobApplicationStatuses>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                Name = "Dev School",
                HR = "Some Teacher",
                Headquarters = "Stockholm"
            },
            new Company
            {
                Id = 2,
                Name = "Nice Small Company",
                HR = "Cool Name",
                Headquarters = "Göteborg"
            }
        );

        modelBuilder.Entity<JobApplication>().HasData(
            new JobApplication
            {
                Id = 1,
                CompanyId = 1,
                Role = "C# Trainee",
                Location = "Stockholm - On Site",
                ApplicationDate = DateOnly.FromDateTime(DateTime.Parse("2025-08-12")),
                StatusId = 4
            },
            new JobApplication
            {
                Id = 2,
                CompanyId = 2,
                Role = "C# Consultant",
                Location = "Göteborg - Full Remote",
                ApplicationDate = DateOnly.FromDateTime(DateTime.Parse("2025-09-12")),
                StatusId = 7
            },
            new JobApplication
            {
                Id = 3,
                Role = "C# Dev",
                Location = "Full Remote",
                ApplicationDate = DateOnly.FromDateTime(DateTime.Parse("2025-09-29")),
                StatusId = 2
            }
        );
        modelBuilder.Entity<JobApplicationStatuses>().HasData(
            new JobApplicationStatuses
            {
                Id = 1,
                Name = "To Apply"
            },
            new JobApplicationStatuses
            {
                Id = 2,
                Name = "Applied"
            },
            new JobApplicationStatuses
            {
                Id = 3,
                Name = "Scheduled Action"
            },
            new JobApplicationStatuses
            {
                Id = 4,
                Name = "Interview"
            },
            new JobApplicationStatuses
            {
                Id = 5,
                Name = "No Answer"
            },
            new JobApplicationStatuses
            {
                Id = 6,
                Name = "Rejected"
            },
            new JobApplicationStatuses
            {
                Id = 7,
                Name = "Discarded"
            },
            new JobApplicationStatuses
            {
                Id = 8,
                Name = "Offer"
            }
        );
    }
}