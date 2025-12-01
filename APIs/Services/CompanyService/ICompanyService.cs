using Shared.Models;

namespace APIs.Services.CompanyService;

public interface ICompanyService
{
    Task<List<Company>> Get();
    Task<List<Company>> GetWithJobApplications();
    Task<Company?> GetById(int id);
    Task<Company?> GetWithJobAppsById(int id);
    Task<Company?> Create(Company newCompany);
    Task<bool> Update(int id, Company updatedCompany);
    Task<bool> Delete(int id);
}