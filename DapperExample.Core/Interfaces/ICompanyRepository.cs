using DapperExample.Core.DTOs;
using DapperExample.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Core.Interfaces;
public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetCompanies();
    Task<Company> GetCompany(int id);
    Task<Company> CreateCompany(CompanyForCreationDto company);
    Task<int> UpdateCompany(int id, CompanyForUpdateDto company);
    Task<int> DeleteCompany(int id);
    Task<Company> GetCompanyByEmployeeId(int id);
    Task<Company> GetMultipleResults(int id);
    Task<List<Company>> MultipleMapping();
    Task<int> CreateMultipleCompanies(List<CompanyForCreationDto> companies);
}
