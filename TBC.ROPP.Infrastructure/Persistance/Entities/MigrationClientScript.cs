namespace TBC.ROPP.Infrastructure.Persistance.Entities;

public enum MigrationClientScriptTypes
{
    PreEfMigration,
    PostEfMigration
}

public class MigrationClientScript
{
    public string Script { get; set; }
    public MigrationClientScriptTypes Type { get; set; }
    public string EfMigration { get; set; }
    public DateTime Date { get; set; }
}