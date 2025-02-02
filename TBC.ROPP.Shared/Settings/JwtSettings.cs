namespace TBC.ROPP.Shared.Settings;

public class JwtSettings
{
    public const string Section = "JwtSettings";

    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string AccessTokenExpirationInSeconds { get; set; } = "120";

    public string SecretKey { get; set; }

}