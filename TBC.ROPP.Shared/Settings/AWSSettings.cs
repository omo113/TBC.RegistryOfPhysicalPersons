namespace TBC.ROPP.Shared.Settings;

public record AWSSettings
{
    public const string SectionName = "AWS";
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
}