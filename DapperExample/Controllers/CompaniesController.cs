using DapperExample.Core.DTOs;
using DapperExample.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    [HttpGet(Name = nameof(GetCompanies))]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _companyService.GetCompanies();
        return Ok(companies);
    }

    [HttpGet("{id}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(int id)
    {
        var company = await _companyService.GetCompany(id);
        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await _companyService.CreateCompany(company);
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyForUpdateDto company)
    {
        await _companyService.UpdateCompany(id, company);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        await _companyService.DeleteCompany(id);
        return NoContent();
    }
    [HttpGet("ByEmployee/{id}")]
    public async Task<IActionResult> GetCompanyForEmployee(int id)
    {
        return Ok(await _companyService.GetCompanyByEmployeeId(id));
    }

    [HttpGet("{id}/MultipleResult")]
    public async Task<IActionResult> GetMultipleResult(int id)
    {
        return Ok(await _companyService.GetMultipleResults(id));
    }
    [HttpGet("MultipleMapping")]
    public async Task<IActionResult> GetMultipleMapping()
    {
        var companies = await _companyService.MultipleMapping();
        return Ok(companies);
    }

    [HttpPost("/MultipleCompanies")]
    public async Task<IActionResult> CreateMultipleCompanies([FromBody] List<CompanyForCreationDto> companies)
    {
        await _companyService.CreateMultipleCompanies(companies);
        return CreatedAtRoute(nameof(GetCompanies), new {});
    }

}
