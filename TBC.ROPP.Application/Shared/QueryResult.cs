namespace TBC.ROPP.Application.Shared;

public record QueryResult<T>
{
    public required long TotalSize { get; init; }
    public required IEnumerable<T> Result { get; init; }
}