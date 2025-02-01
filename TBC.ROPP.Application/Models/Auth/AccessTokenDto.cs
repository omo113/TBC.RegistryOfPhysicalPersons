namespace TBC.ROPP.Application.Models.Auth;

public record AccessTokenDto(string? Issuer, string? Audience, double Expires, string Token);