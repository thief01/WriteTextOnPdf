namespace EasyAddTextToPdf.XMLMigrations;

public abstract class XMLMigrationBase
{
    public abstract int FromVersion { get; }
    public abstract int ToVersion { get; }
    
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