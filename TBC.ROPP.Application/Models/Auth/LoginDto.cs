namespace TBC.ROPP.Application.Models.Auth;

public record LoginDto(AccessTokenDto AccessToken, string RefreshToken);