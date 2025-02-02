using System.Text.RegularExpressions;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands.Helpers;

public static partial class RegexHelpers
{

    const string LatinPattern = @"^[a-zA-Z]+$";
    const string GeorgianPattern = @"^[\u10D0-\u10FF]+$";

    [GeneratedRegex(LatinPattern, RegexOptions.Singleline)]
    public static partial Regex GeneratedLatinRegex();
    [GeneratedRegex(GeorgianPattern, RegexOptions.Singleline)]
    public static partial Regex GeneratedGeorgianRegex();
}