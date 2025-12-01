using APIs.Services.JobApplicationService;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//todo
// [Authorize]
public class JobApplicationsController : ControllerBase
{
    protected readonly IJobApplicationService _jobApplicationService;

    public JobApplicationsController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet(Name = "GetJobApplications")]
    public async Task<ActionResult<List<JobApplicationDto>>> GetJobApplications()
    {
        var jobApplications = await _jobApplicationService.Get();

        var jobApplicationsDto = new List<JobApplicationDto>();

        foreach (var j in jobApplications) jobApplicationsDto.Add(j.Adapt<JobApplication, JobApplicationDto>());

        return Ok(jobApplicationsDto);
    }

    [HttpGet("with-company")]
    public async Task<ActionResult<List<JobApplicationWithCompanyDto>>> GetJobApplicationsWithCompany()
    {
        var jobApplications = await _jobApplicationService.GetWithCompany();

        var jobApplicationsDto = new List<JobApplicationWithCompanyDto>();

        foreach (var j in jobApplications)
            jobApplicationsDto.Add(j.Adapt<JobApplication, JobApplicationWithCompanyDto>());

        return Ok(jobApplicationsDto);
    }

    [HttpGet("with-company-and-status")]
    public async Task<ActionResult<List<JobApplicationWithCompanyDto>>> GetJobApplicationsWithCompanyAndStatus()
    {
        var jobApplications = await _jobApplicationService.GetWithCompanyAndStatus();

        var jobApplicationsDto = new List<JobApplicationWithCompanyDto>();

        foreach (var j in jobApplications)
            jobApplicationsDto.Add(j.Adapt<JobApplication, JobApplicationWithCompanyDto>());

        return Ok(jobApplicationsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplicationDto?>> GetJobApplicationById(int id)
    {
        var jobApplication = await _jobApplicationService.GetById(id);

        if (jobApplication is null)
            return NotFound();

        return Ok(jobApplication.Adapt<JobApplicationDto>());
    }

    [HttpGet("{id}/with-company")]
    public async Task<ActionResult<JobApplicationWithCompanyDto?>> GetJobApplicationWithCompanyById(int id)
    {
        var jobApplication = await _jobApplicationService.GetWithCompanyById(id);

        if (jobApplication is null)
            return NotFound();

        return Ok(jobApplication.Adapt<JobApplicationWithCompanyDto>());
    }

    [HttpGet("{id}/with-company-and-status")]
    public async Task<ActionResult<JobApplicationWithCompanyDto?>> GetJobApplicationWithCompanyAndStatusById(int id)
    {
        var jobApplication = await _jobApplicationService.GetWithCompanyAndStatusById(id);

        if (jobApplication is null)
            return NotFound();

        return Ok(jobApplication.Adapt<JobApplicationWithCompanyDto>());
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> CreateJobApplication(
        JobApplicationRequestDto newJobApplicationDto)
    {
        if (newJobApplicationDto is null)
            return BadRequest();

        var newJobApplication = newJobApplicationDto.Adapt<JobApplication>();

        var jobApplication = await _jobApplicationService.Create(newJobApplication);

        if (jobApplication is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetJobApplicationById), new { id = jobApplication.Id }, newJobApplicationDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateJobApplication(int id, JobApplicationRequestDto updatedJobApplication)
    {
        if (updatedJobApplication.Role is null || updatedJobApplication.Role == "")
            return BadRequest();

        var updateResult = await _jobApplicationService
            .Update(id, updatedJobApplication.Adapt<JobApplicationRequestDto, JobApplication>());

        return updateResult ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteJobApplication(int id)
    {
        var deleteResult = await _jobApplicationService.Delete(id);
        return deleteResult ? NoContent() : NotFound();
    }
}