using Shared.Models;

namespace APIs.Services.JobApplicationStatusService;

public interface IJobApplicationStatusService
{
    Task<List<JobApplicationStatuses>> Get();
    Task<JobApplicationStatuses?> GetById(int id);
    Task<JobApplicationStatuses?> Create(JobApplicationStatuses newStatus);
    Task<bool> Update(int id, JobApplicationStatuses updatedStatus);
    Task<bool> Delete(int id);
}