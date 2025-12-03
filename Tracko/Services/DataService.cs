using System.Net.Http.Json;
using Mapster;
using Shared.Models;

namespace Tracko.Services;

public class DataService(HttpClient http)
{
    public async Task<List<JobApplicationWithCompanyDto>?> GetJobApplicationsAsync()
    {
        return await http.GetFromJsonAsync<List<JobApplicationWithCompanyDto>>("JobApplications/with-company");
    }

    public async Task<JobApplicationRequestDto?> GetJobApplicationByIdAsync(int jobApplicationId)
    {
        var jobApp =
            await http.GetFromJsonAsync<JobApplicationWithCompanyDto>(
                $"JobApplications/{jobApplicationId}/with-company");
        return jobApp?.Adapt<JobApplicationWithCompanyDto, JobApplicationRequestDto>();
    }

    public async Task CreateJobApplicationAsync(JobApplicationRequestDto jobApplication)
    {
        await http.PostAsJsonAsync("JobApplications", jobApplication);
    }

    public async Task UpdateJobApplicationAsync(JobApplicationRequestDto updatedJobApplication)
    {
        await http.PutAsJsonAsync($"JobApplications/{updatedJobApplication.Id}", updatedJobApplication);
    }

    public async Task DeleteJobApplicationAsync(int id)
    {
        await http.DeleteAsync($"JobApplications/{id}");
    }

    public async Task<List<CompanyDto>?> GetCompaniesAsync()
    {
        return await http.GetFromJsonAsync<List<CompanyDto>>("Companies");
    }
}