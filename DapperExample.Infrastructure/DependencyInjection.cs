using DapperExample.Core.Interfaces;
using DapperExample.Infrastructure.Context;
using DapperExample.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        return services;
    }
}
