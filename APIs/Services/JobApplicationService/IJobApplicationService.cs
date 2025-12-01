using Shared.Models;

namespace APIs.Services.JobApplicationService;

public interface IJobApplicationService
{
    Task<List<JobApplication>> Get();
    Task<List<JobApplication>> GetWithCompany();
    Task<List<JobApplication>> GetWithCompanyAndStatus();
    Task<JobApplication?> GetById(int id);
    Task<JobApplication?> GetWithCompanyById(int id);
    Task<JobApplication?> GetWithCompanyAndStatusById(int id);
    Task<JobApplication?> Create(JobApplication newJobApplication);
    Task<bool> Update(int id, JobApplication updatedJobApplication);
    Task<bool> Delete(int id);
}