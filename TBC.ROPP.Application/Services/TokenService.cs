using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TBC.ROPP.Application.Models.Auth;
using TBC.ROPP.Application.Services.Abstractions;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Shared.Settings;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TBC.ROPP.Application.Services;



public class TokenService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettingsOptions) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettingsOptions.Value;
    public async Task<AccessTokenDto> GenerateJwtToken(ApplicationUser user)
    {
        var role = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
                     {
                         new(JwtRegisteredClaimNames.Sub, user.UserName),
                         new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                         new ("role",role[0])
                         // Add other claims as needed
                     };

        var userClaims = await userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.AccessTokenExpirationInSeconds));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessTokenDto(tokenDescriptor.Issuer, tokenDescriptor.Audience, Convert.ToDouble(_jwtSettings.AccessTokenExpirationInSeconds) * 60, tokenHandler.WriteToken(token));
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}