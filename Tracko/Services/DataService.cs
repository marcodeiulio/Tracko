using System.Net.Http.Json;
using Shared.Models;

namespace Tracko.Services;

public class DataService(HttpClient http)
{
    public async Task<List<JobApplicationWithCompanyDto>?> GetJobApplications()
    {
        return await http.GetFromJsonAsync<List<JobApplicationWithCompanyDto>>("JobApplications/with-company");
    }

    public async Task<List<CompanyDto>?> GetCompanies()
    {
        return await http.GetFromJsonAsync<List<CompanyDto>>("Companies");
    }
}