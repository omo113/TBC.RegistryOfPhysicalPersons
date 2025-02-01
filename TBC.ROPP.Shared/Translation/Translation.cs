using Microsoft.Extensions.Logging;
using System.Resources;

namespace TBC.ROPP.Shared.Translation;

public class Translation
{
    private static ResourceManager _customErrorResourceMan;
    private static readonly ILogger Logger;

    static Translation()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        Logger = loggerFactory.CreateLogger(nameof(Translation));
    }

    public static ResourceManager CustomErrorMessageResourceManager
    {
        get
        {
            if (!ReferenceEquals(_customErrorResourceMan, null)) return _customErrorResourceMan;

            var temp = new ResourceManager("DailyReports.Shared.Translation.Resources.Translations", typeof(Translation).Assembly);
            _customErrorResourceMan = temp;
            return _customErrorResourceMan;
        }
    }

    public static string? Translate(string name, params object?[] parameters)
    {
        if (parameters.Length == 0)
        {
            try
            {
                return CustomErrorMessageResourceManager.GetString(name);
            }
            catch (Exception e)
            {
                Logger.LogError(e,
                                "cannot format string with this parameters  for translation with key {Name} for parameters: {Parameters}",
                                name, parameters);
                return string.Empty;
            }
        }

        var toReturn = CustomErrorMessageResourceManager.GetString(name);
        if (toReturn == null) return toReturn;

        try
        {
            return string.Format(toReturn, parameters);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "cannot format string with this parameters  for translation with key {Name} for parameters: {Parameters}",
                            name, parameters);
            return string.Empty;
        }
    }
    public static void EnsureTranslationServiceWorks()
    {
        try
        {
            var customTranslation = CustomErrorMessageResourceManager.GetString("test");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Translation service encountered an error during startup validation");
            throw;
        }
    }
}
