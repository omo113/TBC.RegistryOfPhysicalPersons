namespace TBC.ROPP.Application.Shared;

public class Query<T> : Query
{
    public T Params { get; init; } = Activator.CreateInstance<T>();
}



public class Query
{
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
}