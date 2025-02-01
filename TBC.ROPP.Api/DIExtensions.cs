using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

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

}