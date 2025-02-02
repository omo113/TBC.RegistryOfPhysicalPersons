namespace TBC.ROPP.Application.Shared;

public static class CultureUtils
{
    public static string? GetCurrentCultureValue(bool isGeorgian, string? englishValue, string? georgianValue)
        => isGeorgian ? georgianValue : englishValue;

    public static bool GetCurrentCultureIsGeorgian()
        => Thread.CurrentThread.CurrentCulture.Name == "ka-GE";
}