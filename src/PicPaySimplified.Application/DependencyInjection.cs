using Microsoft.Extensions.DependencyInjection;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Application.Services;

namespace PicPaySimplified.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}