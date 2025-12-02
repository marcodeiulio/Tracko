namespace Shared.Models;

public class Company
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public string? HR { get; set; }
    public string? Headquarters { get; set; }
    public string? Notes { get; set; }

    public List<JobApplication>? JobApplications { get; set; }
}

public class CompanyDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? HR { get; set; }
    public string? Headquarters { get; set; }
    public string? Notes { get; set; }
}

public class CompanyWithJobApplicationDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? HR { get; set; }
    public string? Headquarters { get; set; }
    public string? Notes { get; set; }

    public List<JobApplicationDto>? JobApplications { get; set; }
}

public class CompanyRequestDto
{
    public required string Name { get; set; }
    public string? HR { get; set; }
    public string? Headquarters { get; set; }
    public string? Notes { get; set; }
}