using APIs.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace APIs.Services.JobApplicationService;

public class JobApplicationService(ApplicationDbContext context) : IJobApplicationService
{
    public async Task<List<JobApplication>> Get()
    {
        return await context.JobApplications
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<JobApplication>> GetWithCompany()
    {
        return await context.JobApplications
            .Include(j => j.Company)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<JobApplication>> GetWithCompanyAndStatus()
    {
        return await context.JobApplications
            .Include(j => j.Company)
            .Include(j => j.Status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<JobApplication?> GetById(int id)
    {
        return await context.JobApplications
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id) ?? null;
    }

    public async Task<JobApplication?> GetWithCompanyById(int id)
    {
        return await context.JobApplications
            .Include(j => j.Company)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id) ?? null;
    }

    public async Task<JobApplication?> GetWithCompanyAndStatusById(int id)
    {
        return await context.JobApplications
            .Include(j => j.Company)
            .Include(j => j.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id) ?? null;
    }

    public async Task<JobApplication?> Create(JobApplication newJobApplication)
    {
        if (newJobApplication.Role is null || newJobApplication.Role == "")
            return null;

        try
        {
            context.JobApplications.Add(newJobApplication);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return null;
        }

        return newJobApplication;
    }

    public async Task<bool> Update(int id, JobApplication updatedJobApplication)
    {
        if (updatedJobApplication.Role is null || updatedJobApplication.Role == "")
            return false;

        var jobApplicationToUpdate = await context.JobApplications
            .FirstOrDefaultAsync(j => j.Id == id);
        if (jobApplicationToUpdate is null)
            return false;

        jobApplicationToUpdate.Role = updatedJobApplication.Role;
        jobApplicationToUpdate.Location = updatedJobApplication.Location;
        jobApplicationToUpdate.ApplicationDate = updatedJobApplication.ApplicationDate;
        jobApplicationToUpdate.StatusId = updatedJobApplication.StatusId;
        jobApplicationToUpdate.CompanyId = updatedJobApplication.CompanyId;
        jobApplicationToUpdate.JobDescription = updatedJobApplication.JobDescription;
        jobApplicationToUpdate.CoverLetter = updatedJobApplication.CoverLetter;
        jobApplicationToUpdate.Notes = updatedJobApplication.Notes;

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
        var jobApplicationToDelete = await GetById(id);
        if (jobApplicationToDelete is null)
            return false;

        try
        {
            context.JobApplications.Remove(jobApplicationToDelete);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}