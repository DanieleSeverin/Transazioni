using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transazioni.Application.Abstractions.Authentication;
using Transazioni.Application.CheBanca.UploadCheBancaMovements;
using Transazioni.Application.Fideuram.UploadFideuramMovements;
using Transazioni.Application.Paypal.UploadPaypalMovements;
using Transazioni.Application.Reporting.GetAccountsBalance;
using Transazioni.Application.Reporting.GetCosts;
using Transazioni.Application.Reporting.GetRevenue;
using Transazioni.Application.Santander.uploadSantanderMovements;
using Transazioni.Application.Satispay.UploadSatispayMovements;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Users;
using Transazioni.Infrastructure.Authentication.Jwt;
using Transazioni.Infrastructure.Authentication.PasswordHandler;
using Transazioni.Infrastructure.Repositories;
using Transazioni.Infrastructure.Services.DataReaders;
using Transazioni.Infrastructure.Services.Reporting;

namespace Transazioni.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistence(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        //var connectionString = builder.Configuration.GetConnectionString("Database") ??
        //            throw new ArgumentNullException(nameof(builder.Configuration));

        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_Database") ??
            configuration.GetConnectionString("POSTGRESQLCONNSTR_Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            //options.UseSqlServer(connectionString); // SqlServer
            options.UseNpgsql(connectionString); // PostgreSQL
        });

        AddRepositories(services);
        AddServices(services);

    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountRuleRepository, AccountRuleRepository>();
        services.AddScoped<IMovementsRepository, MovementsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICheBancaReader, CheBancaReader>();
        services.AddScoped<IFideuramReader, FideuramReader>();
        services.AddScoped<IPaypalReader, PaypalReader>();
        services.AddScoped<ISatispayReader, SatispayReader>();
        services.AddScoped<ISantanderReader, SantanderReader>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordEncrypter, PasswordEncrypter>();
        services.AddScoped<IAccountBalanceProvider, AccountBalanceProvider>();
        services.AddScoped<ICostsProvider, CostsProvider>();
        services.AddScoped<IRevenueProvider, RevenueProvider>();
    }
}
