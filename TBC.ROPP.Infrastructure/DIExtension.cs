﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Infrastructure.FileStorage;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.Persistance.Abstractions;
using TBC.ROPP.Infrastructure.Repositories;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.Settings;

namespace TBC.ROPP.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var applicationSettings = sp.GetRequiredService<IOptions<ApplicationSettings>>();
            options.UseSqlServer(applicationSettings.Value.DatabaseConnection);
        });
        services.AddIdentityCore<ApplicationUser>()
        .AddRoles<ApplicationRole>()
        .AddUserManager<UserManager<ApplicationUser>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IMigrationDbContext, ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddFileInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, S3FileStorage>();
        services.AddScoped<IS3Bucket, S3FileStorage>();
        return services;
    }
}