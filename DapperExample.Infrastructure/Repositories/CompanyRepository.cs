using Dapper;
using DapperExample.Core.DTOs;
using DapperExample.Core.Entities;
using DapperExample.Core.Interfaces;
using DapperExample.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Infrastructure.Repositories;
public class CompanyRepository : ICompanyRepository
{
    private readonly DapperContext _context;

    public CompanyRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Company> CreateCompany(CompanyForCreationDto company)
    {
        var sql = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                   "SELECT CAST(SCOPE_IDENTITY() AS int)";
        var parameters = new DynamicParameters();
        parameters.Add("Name", company.Name, DbType.String);
        parameters.Add("Address", company.Address, DbType.String);
        parameters.Add("Country", company.Country, DbType.String);

        using (var connection = _context.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(sql, parameters);
            var createdCompany = new Company
            {
                Id = id,
                Name = company.Name,
                Address = company.Address,
                Country = company.Country,
            };
            return createdCompany;
        }
    }

    public async Task<int> CreateMultipleCompanies(List<CompanyForCreationDto> companies)
    {
        var sql = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)";
        using(var connection = _context.CreateConnection())
        {
            connection.Open();
            using(var transaction = connection.BeginTransaction())
            {
                try
                {
                    int affectedRows = 0;
                    foreach(var company in companies)
                    {
                        affectedRows += await connection.ExecuteAsync(sql, company, transaction:transaction);
                    }
                    transaction.Commit();
                    return affectedRows;

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<int> DeleteCompany(int id)
    {
        var sql = "DELETE FROM Companies WHERE Id = @Id";
        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, new {id});
            return affectedRows;
        }

    }

    public async Task<IEnumerable<Company>> GetCompanies()
    {
        var sql = "SELECT * FROM Companies";
        using (var connection = _context.CreateConnection())
        {
            var companies = await connection.QueryAsync<Company>(sql);
            return companies.ToList();
        }
    }

    public async Task<Company> GetCompany(int id)
    {
        var sql = "SELECT * FROM Companies Where Id = @Id";
        using (var connection = _context.CreateConnection())
        {
            var company = await connection.QuerySingleOrDefaultAsync<Company>(sql, new { id });
            return company;
        }
    }

    public async Task<Company> GetCompanyByEmployeeId(int id)
    {
        var sql = "sp_ShowCompanyByEmployeeID";
        using(var connection = _context.CreateConnection())
        {
            var company  = await connection.QueryFirstOrDefaultAsync<Company>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return company;
        }
    }

    public async Task<Company> GetMultipleResults(int id)
    {
        var sql = "SELECT * FROM Companies WHERE Id = @Id;" +
                  "SELECT * FROM Employees WHERE CompanyId = @Id";
        using(var connection = _context.CreateConnection())
        using(var multy = await connection.QueryMultipleAsync(sql, new { id }))
        {
            var company = await multy.ReadSingleOrDefaultAsync<Company>();
            if(company is not null)
            {
                company.Employees = (await multy.ReadAsync<Employee>()).ToList();
            }
            return company;
        }
    }

    public async Task<List<Company>> MultipleMapping()
    {
        var sql = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";
        var companyDict = new Dictionary<int, Company>();
        using(var connection = _context.CreateConnection())
        {
            var companies = await connection.QueryAsync<Company, Employee, Company>(sql, (company, employee) =>
            {
                if(!companyDict.TryGetValue(company.Id, out var currentCompany))
                {
                    currentCompany = company;
                    companyDict.Add(company.Id, company);
                }
                currentCompany.Employees.Add(employee);
                return currentCompany;
            });

            return companies.Distinct().ToList();
        }
    }

    public async Task<int> UpdateCompany(int id, CompanyForUpdateDto company)
    {
        var sql = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);
        parameters.Add("Name", company.Name, DbType.String);
        parameters.Add("Address", company.Address, DbType.String);
        parameters.Add("Country", company.Country, DbType.String);
        using(var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows;
        }
    }
}
