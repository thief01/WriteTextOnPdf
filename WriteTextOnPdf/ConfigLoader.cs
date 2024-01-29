using System.Numerics;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WriteTextOnPdf.XML;
using WriteTextOnPdf.XML.Migrations;
using Rectangle = iTextSharp.text.Rectangle;

namespace WriteTextOnPdf
{
    public class ConfigLoader
    {
        public const string CORE_SETTINGS_FILE_NAME = "core-config.xml";
        public const string SETTINGS_FILE_NAME = "config.xml";
        
        public static XmlDocument XmlDocument { get; private set; }
        public static ConfigLoader Instance => instance;
        private static ConfigLoader instance;
        
        public string LoadedConfig { get; private set; }
        
        public ConfigLoader()
        {
            if(instance != null)
                return;
            instance = this;
            LoadSettings();
        }
        
        public void ReloadSettings()
        {
            LoadSettings();
        }
        
        private void LoadSettings()
        {
            if (File.Exists(SETTINGS_FILE_NAME))
            {
                LoadSettings(SETTINGS_FILE_NAME);
                return;
            }

            if (File.Exists(CORE_SETTINGS_FILE_NAME))
            {
                LoadSettings(CORE_SETTINGS_FILE_NAME);
                return;
            }

            Console.Write("Config doesn't exist.");
        }
        
        private void LoadSettings(string config)
        {
            XmlDocument = new XmlDocument();
            XmlDocument.Load(config);
            LoadedConfig = config;
            var ver =XmlDocument.ReadString("base", "Config/XMLVersion");
            Console.WriteLine("Migrating XML");
            if (ver == "base")
            {
                XMLMigrationFromBaseToVer1 migration = new XMLMigrationFromBaseToVer1();
                migration.Migrate();
            }
            Console.WriteLine("Migration done");
            
        }
    }
}