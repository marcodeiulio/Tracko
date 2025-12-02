namespace Shared.Models;

public class JobApplicationStatuses
{
    public int Id { get; init; }
    public required string Name { get; set; }
}

public class JobApplicationStatusesDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class JobApplicationStatusesRequestDto
{
    public required string Name { get; set; }
}