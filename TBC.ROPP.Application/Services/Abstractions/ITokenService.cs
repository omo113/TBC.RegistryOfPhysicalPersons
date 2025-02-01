using TBC.ROPP.Domain.IdentityEntities;

namespace TBC.ROPP.Application.Services.Abstractions;

public interface ITokenService
{
    string GenerateRefreshToken();
    Task<AccessTokenDto> GenerateJwtToken(ApplicationUser user);
}