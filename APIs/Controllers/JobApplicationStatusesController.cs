using APIs.Services.JobApplicationStatusService;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//todo
// [Authorize]
public class JobApplicationStatusesController : ControllerBase
{
    protected readonly IJobApplicationStatusService _jobApplicationStatusService;

    public JobApplicationStatusesController(IJobApplicationStatusService jobApplicationStatusService)
    {
        _jobApplicationStatusService = jobApplicationStatusService;
    }

    [HttpGet(Name = "GetJobApplicationStatuses")]
    public async Task<ActionResult<List<JobApplicationStatusesDto>>> GetJobApplicationStatuses()
    {
        var statuses = await _jobApplicationStatusService.Get();

        var statusesDto = new List<JobApplicationStatusesDto>();

        foreach (var s in statuses) statusesDto.Add(s.Adapt<JobApplicationStatuses, JobApplicationStatusesDto>());

        return Ok(statusesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplicationStatusesDto?>> GetJobApplicationStatusById(int id)
    {
        var status = await _jobApplicationStatusService.GetById(id);

        if (status is null)
            return NotFound();

        return Ok(status.Adapt<JobApplicationStatusesDto>());
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationStatusesDto>> CreateJobApplicationStatus(
        JobApplicationStatusesRequestDto newStatusDto)
    {
        if (newStatusDto is null)
            return BadRequest();

        var newStatus = newStatusDto.Adapt<JobApplicationStatuses>();

        var status = await _jobApplicationStatusService.Create(newStatus);

        if (status is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetJobApplicationStatusById), new { id = status.Id }, newStatusDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateJobApplicationStatus(int id, JobApplicationStatusesRequestDto updatedStatus)
    {
        if (updatedStatus.Name is null || updatedStatus.Name == "")
            return BadRequest();

        var updateResult = await _jobApplicationStatusService
            .Update(id, updatedStatus.Adapt<JobApplicationStatusesRequestDto, JobApplicationStatuses>());

        return updateResult ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteJobApplicationStatus(int id)
    {
        var deleteResult = await _jobApplicationStatusService.Delete(id);
        return deleteResult ? NoContent() : NotFound();
    }
}