using APIs.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace APIs.Services.CompanyService;

public class CompanyService(ApplicationDbContext context) : ICompanyService
{
    public async Task<List<Company>> Get()
    {
        return await context.Companies
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Company>> GetWithJobApplications()
    {
        return await context.Companies
            .Include(c => c.JobApplications)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Company?> GetById(int id)
    {
        return await context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id) ?? null;
    }

    public async Task<Company?> GetWithJobAppsById(int id)
    {
        return await context.Companies
            .Include(c => c.JobApplications)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id) ?? null;
    }

    public async Task<Company?> Create(Company newCompany)
    {
        if (newCompany.Name is null || newCompany.Name == "")
            return null;

        try
        {
            context.Companies.Add(newCompany);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return null;
        }

        return newCompany;
    }

    public async Task<bool> Update(int id, Company updatedCompany)
    {
        if (updatedCompany.Name is null || updatedCompany.Name == "")
            return false;

        var companyToUpdate = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);
        if (companyToUpdate is null)
            return false;

        companyToUpdate.Name = updatedCompany.Name;
        companyToUpdate.HR = updatedCompany.HR;
        companyToUpdate.Headquarters = updatedCompany.Headquarters;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var companyToDelete = await GetById(id);
        if (companyToDelete is null)
            return false;

        try
        {
            context.Companies.Remove(companyToDelete);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}