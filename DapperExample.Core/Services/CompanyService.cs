using DapperExample.Core.DTOs;
using DapperExample.Core.Entities;
using DapperExample.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Core.Services;
public class CompanyService : ICompanyService
{
	private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Company> CreateCompany(CompanyForCreationDto company)
    {
        try
        {
            return await _companyRepository.CreateCompany(company);
        }
        catch
        {

            throw;
        }
    }

    public async Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
    {
		var affectedRows = await _companyRepository.CreateMultipleCompanies(companies);
		Console.WriteLine(affectedRows);
    }

    public async Task DeleteCompany(int id)
    {
		try
		{
            await ChechIfCompanyExist(id);
			var result  = await _companyRepository.DeleteCompany(id);

        }
		catch 
		{

			throw;
		}
    }

    public async Task<IEnumerable<Company>> GetCompanies()
    {
		try
		{
			return await _companyRepository.GetCompanies();
		}
		catch
		{

			throw;
		}
    }

    public async Task<Company> GetCompany(int id)
    {
		try
		{
			var company = await _companyRepository.GetCompany(id);
			if(company is null)
			{
				throw new Exception($"Company with id {id} are not found");
			}
			return company;
		}
		catch 
		{

			throw;
		}
    }

    public async Task<Company> GetCompanyByEmployeeId(int id)
    {
        var company =  await _companyRepository.GetCompanyByEmployeeId(id);
		if(company is null)
		{
			throw new Exception("Company is not found");
		}
		return company;
    }

    public async Task<Company> GetMultipleResults(int id)
    {
        var company =  await _companyRepository.GetMultipleResults(id);
		if(company is null)
		{
			throw new Exception($"Company with id {id} are not found");
		}
		return company;
    }

    public Task<List<Company>> MultipleMapping()
    {
        return _companyRepository.MultipleMapping();
    }

    public async Task UpdateCompany(int id, CompanyForUpdateDto company)
    {
		try
		{
			await ChechIfCompanyExist(id);
			
			var result = await _companyRepository.UpdateCompany(id, company);
			
		}
		catch
		{

			throw;
		}
    }

    private async Task ChechIfCompanyExist(int id)
    {
        await GetCompany(id);
    }
}
