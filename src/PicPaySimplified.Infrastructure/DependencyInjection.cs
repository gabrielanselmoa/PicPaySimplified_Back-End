using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Interfaces;
using PicPaySimplified.Infrastructure.Http;
using PicPaySimplified.Infrastructure.Repositories;
using PicPaySimplified.Infrastructure.Security;

namespace PicPaySimplified.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // DB Connection
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
        
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        // Services
        services.AddScoped<IExternalAuthorizationService,  ExternalAuthorizationService>();
        services.AddScoped<IExternalNotificationService, ExternalNotificationService>();
        services.AddScoped<ISecurityService, SecurityService>();
        return services;
    }
}