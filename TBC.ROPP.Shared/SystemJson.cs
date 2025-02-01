using System.Text.Json;
using System.Text.Json.Serialization;

namespace TBC.ROPP.Shared;

public class SystemJson
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, };
}
