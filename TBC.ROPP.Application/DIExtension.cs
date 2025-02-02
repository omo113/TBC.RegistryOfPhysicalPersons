using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TBC.ROPP.Application.Behaviors;
using TBC.ROPP.Application.Services;
using TBC.ROPP.Application.Services.Abstractions;

namespace TBC.ROPP.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<ITokenService, TokenService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), filter: result =>
        {
            foreach (var constructor in result.ValidatorType.GetConstructors())
            {
                return !constructor.GetParameters()
                    .Any(x => x.ParameterType.IsPrimitive);
            }

            return true;
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IFileStorageService, FileStorageService>();
        return services;
    }
}