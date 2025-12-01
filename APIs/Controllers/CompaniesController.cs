using APIs.Services.CompanyService;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//todo
// [Authorize]
public class CompaniesController : ControllerBase
{
    protected readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet(Name = "GetCompanies")]
    public async Task<ActionResult<List<CompanyDto>>> GetCompanies()
    {
        var companies = await _companyService.Get();

        var companiesDto = new List<CompanyDto>();

        foreach (var c in companies) companiesDto.Add(c.Adapt<Company, CompanyDto>());

        return Ok(companiesDto);
    }

    [HttpGet("with-job-applications")]
    public async Task<ActionResult<List<CompanyWithJobApplicationDto>>> GetCompaniesWithJobApps()
    {
        var companies = await _companyService.GetWithJobApplications();

        var companiesDto = new List<CompanyWithJobApplicationDto>();

        foreach (var c in companies)
        {
            if (c.JobApplications is not null)
            {
                var jobAppsDto = new List<JobApplicationDto>();
                foreach (var j in c.JobApplications) j.Adapt<JobApplicationDto>();
            }

            companiesDto.Add(c.Adapt<Company, CompanyWithJobApplicationDto>());
        }

        return Ok(companiesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto?>> GetCompanyById(int id)
    {
        var company = await _companyService.GetById(id);

        if (company is null)
            return NotFound();

        return Ok(company.Adapt<CompanyDto>());
    }

    [HttpGet("{id}/with-job-applications")]
    public async Task<ActionResult<CompanyWithJobApplicationDto?>> GetCompanyWithJobAppsById(int id)
    {
        var company = await _companyService.GetWithJobAppsById(id);

        if (company is null)
            return NotFound();

        if (company.JobApplications is not null)
            foreach (var j in company.JobApplications)
                j.Adapt<JobApplicationDto>();

        return Ok(company.Adapt<CompanyWithJobApplicationDto>());
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyRequestDto newCompanyDto)
    {
        if (newCompanyDto is null)
            return BadRequest();

        var newCompany = newCompanyDto.Adapt<Company>();

        var company = await _companyService.Create(newCompany);

        if (company is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, newCompanyDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCompany(int id, CompanyRequestDto updatedCompany)
    {
        //todo
        if (updatedCompany.Name is null || updatedCompany.Name == "")
            return BadRequest();

        var updateResult = await _companyService
            .Update(id, updatedCompany.Adapt<CompanyRequestDto, Company>());

        return updateResult ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompany(int id)
    {
        var deleteResult = await _companyService.Delete(id);
        return deleteResult ? NoContent() : NotFound();
    }
}