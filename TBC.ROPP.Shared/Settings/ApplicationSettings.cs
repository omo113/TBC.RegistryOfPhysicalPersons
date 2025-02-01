namespace TBC.ROPP.Shared.Settings;

public class ApplicationSettings
{
    public const string Section = "ApplicationSettings";
    public string BaseFilePath { get; set; }
    public string DatabaseConnection { get; set; }
}