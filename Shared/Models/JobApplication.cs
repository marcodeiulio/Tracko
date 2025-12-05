using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class JobApplication
{
    public int Id { get; init; }
    public required string Role { get; set; }
    public string? Location { get; set; }
    public DateOnly? ApplicationDate { get; set; }

    public int? StatusId { get; set; }
    public JobApplicationStatuses? Status { get; set; }

    public int? CompanyId { get; set; }
    public Company? Company { get; set; }

    public string? JobDescription { get; set; }
    public string? CoverLetter { get; set; }

    public string? Notes { get; set; }
    // todo
    // link to jobapp
}

public class JobApplicationDto
{
    public int Id { get; set; }
    public required string Role { get; set; }
    public string? Location { get; set; }
    public DateOnly? ApplicationDate { get; set; }

    public JobApplicationStatusesDto? Status { get; set; }

    public string? JobDescription { get; set; }
    public string? CoverLetter { get; set; }
    public string? Notes { get; set; }
}

public class JobApplicationWithCompanyDto
{
    public int Id { get; set; }
    public required string Role { get; set; }
    public string? Location { get; set; }
    public DateOnly? ApplicationDate { get; set; }

    public JobApplicationStatusesDto? Status { get; set; }

    public CompanyDto? Company { get; set; }

    public string? JobDescription { get; set; }
    public string? CoverLetter { get; set; }
    public string? Notes { get; set; }
}

public class JobApplicationRequestDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "The Role field is required for a job application.")]
    [StringLength(100, ErrorMessage = "Role field cannot exceed 100 characters.")]
    public string Role { get; set; }

    [StringLength(100, ErrorMessage = "Location field cannot exceed 100 characters.")]
    public string? Location { get; set; }

    [DataType(DataType.Date)] public DateOnly? ApplicationDate { get; set; }

    public int? CompanyId { get; set; }
    public CompanyDto? Company { get; set; }
    public JobApplicationStatusesDto? Status { get; set; }

    [StringLength(5000, ErrorMessage = "Job Description field cannot exceed 5000 characters")]
    public string? JobDescription { get; set; }

    [StringLength(5000, ErrorMessage = "Cover Letter field cannot exceed 5000 characters")]
    public string? CoverLetter { get; set; }

    [StringLength(5000, ErrorMessage = "Notes field cannot exceed 5000 characters")]
    public string? Notes { get; set; }
}

public class JobApplicationViewDto
{
    public int Id { get; set; }

    public required string Role { get; set; }

    public string? Location { get; set; }

    [DataType(DataType.Date)] public DateOnly? ApplicationDate { get; set; }

    public string FormattedApplicationDate =>
        ApplicationDate?.ToString("dd/MM/yyyy") ?? "";

    public CompanyDto? Company { get; set; }
    public int? CompanyId { get; set; }
    public JobApplicationStatusesDto? Status { get; set; }

    public string? JobDescription { get; set; }

    public string? FormattedJobDescription
    {
        get
        {
            if (string.IsNullOrEmpty(JobDescription)) return string.Empty;
            var preview = JobDescription.Substring(0, Math.Min(JobDescription.Length, 100));
            if (JobDescription.Length > 100) return preview + "...";
            return preview;
        }
    }

    public string? CoverLetter { get; set; }

    public string? FormattedCoverLetter
    {
        get
        {
            if (string.IsNullOrEmpty(CoverLetter)) return string.Empty;
            var preview = CoverLetter.Substring(0, Math.Min(CoverLetter.Length, 100));
            if (CoverLetter.Length > 100) return preview + "...";
            return preview;
        }
    }

    public string? Notes { get; set; }

    public string? FormattedNotes
    {
        get
        {
            if (string.IsNullOrEmpty(Notes)) return string.Empty;
            var preview = Notes.Substring(0, Math.Min(Notes.Length, 100));
            if (Notes.Length > 100) return preview + "...";
            return preview;
        }
    }
}