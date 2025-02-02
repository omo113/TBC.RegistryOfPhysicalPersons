using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text;
using TBC.ROPP.Shared.Translation;

namespace TBC.ROPP.Api;

public static class DIExtensions
{
    public static IApplicationBuilder AddRequestLocalization(this IApplicationBuilder builder)
    {
        var supportedCultures = new List<CultureInfo> { new("en-US"), new("ka-GE"), };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture("en-US")
            .AddSupportedUICultures(supportedCultures.Select(x => x.Name).ToArray());

        builder.UseRequestLocalization(localizationOptions);

        return builder;
    }
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1) // Optional, reduce or remove clock skew
                };
            });
        return services;
    }
    public static IServiceCollection AddLocalizationService(this IServiceCollection services)
    {
        services.AddLocalization();
        Translation.EnsureTranslationServiceWorks();
        return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
         {
             options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
             {
                 Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer"
             });

             options.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
             });

             options.OperationFilter<AddRequiredHeadersOperationFilter>();
         });
        return services;
    }
}

public class AddRequiredHeadersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Ensure parameters list exists
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // Add the custom "culture" header parameter
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "culture",
            In = ParameterLocation.Header,
            Description = "Culture header for the request (e.g., en-US, fr-FR)",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}
