using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TBC.ROPP.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}