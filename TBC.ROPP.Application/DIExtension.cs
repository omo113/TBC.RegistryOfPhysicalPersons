using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TBC.ROPP.Application.Services;
using TBC.ROPP.Application.Services.Abstractions;

namespace TBC.ROPP.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}