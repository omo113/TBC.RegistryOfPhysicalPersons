namespace TBC.ROPP.MigrationClient.Shared;

public class MigrationScriptFailedException : Exception
{
    public MigrationScriptFailedException(string name)
        : base($"Migration {name}: Failed!")
    {
    }

    public MigrationScriptFailedException(string name, string message)
        : base($"Migration {name}: Failed! {message}")
    {
    }
}