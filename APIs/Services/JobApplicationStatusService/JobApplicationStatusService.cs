using APIs.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace APIs.Services.JobApplicationStatusService;

public class JobApplicationStatusService(ApplicationDbContext context) : IJobApplicationStatusService
{
    public async Task<List<JobApplicationStatuses>> Get()
    {
        return await context.JobApplicationStatuses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<JobApplicationStatuses?> GetById(int id)
    {
        return await context.JobApplicationStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id) ?? null;
    }

    public async Task<JobApplicationStatuses?> Create(JobApplicationStatuses newStatus)
    {
        if (newStatus.Name is null || newStatus.Name == "")
            return null;

        try
        {
            context.JobApplicationStatuses.Add(newStatus);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return null;
        }

        return newStatus;
    }

    public async Task<bool> Update(int id, JobApplicationStatuses updatedStatus)
    {
        if (updatedStatus.Name is null || updatedStatus.Name == "")
            return false;

        var statusToUpdate = await context.JobApplicationStatuses
            .FirstOrDefaultAsync(s => s.Id == id);
        if (statusToUpdate is null)
            return false;

        statusToUpdate.Name = updatedStatus.Name;

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
        var statusToDelete = await GetById(id);
        if (statusToDelete is null)
            return false;

        try
        {
            context.JobApplicationStatuses.Remove(statusToDelete);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}