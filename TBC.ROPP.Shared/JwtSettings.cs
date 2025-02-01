namespace TBC.ROPP.Shared;

public class JwtSettings
{
    public const string Section = "JwtSettings";

    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string AccessTokenExpirationInMinutes { get; set; }

    public string SecretKey { get; set; }

}