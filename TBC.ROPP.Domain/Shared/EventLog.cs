using System.Text.Json;
using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Shared;

namespace TBC.ROPP.Domain.Shared;

public class EventLog : Entity
{
    public string MessageType { get; private set; }
    public string StreamId { get; private set; }
    public JsonElement Data { get; private set; }
    public bool IsPublished { get; set; }

    private EventLog()
    {
    }

    public EventLog(string messageType, string streamId, object data)
    {
        MessageType = messageType;
        StreamId = streamId;
        Data = JsonSerializer.SerializeToElement(data, SystemJson.JsonSerializerOptions);
        CreateDate = SystemDate.Now;
        UId = Guid.NewGuid();
    }
}
