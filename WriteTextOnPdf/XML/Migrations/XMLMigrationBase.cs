namespace WriteTextOnPdf.XML.Migrations;

public abstract class XMLMigrationBase
{
    public abstract string FromVersion { get; }
    public abstract string ToVersion { get; }
    
    public void Migrate()
    {
        LoadSettings();
        MigrateSettings();
        SaveSettings();
    }

    public abstract void LoadSettings();
    
    public abstract void MigrateSettings();
    
    public abstract void SaveSettings();
}