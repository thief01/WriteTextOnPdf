using System.Numerics;
using System.Xml;

namespace WriteTextOnPdf.XML.Migrations;

public class XMLMigrationFromBaseToVer1 : XMLMigrationBase
{
    public override string FromVersion => "base";
    public override string ToVersion => "1.0";
    
    public override void MigrateSettings()
    {
        Console.WriteLine("You have OLD XMLversion but migration system not added yet.");
        Console.ReadKey();
        Console.ReadKey();
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Todo: migration logic
    /// </summary>
    public override void LoadSettings()
    {
        XmlDocument xmlReader = new XmlDocument();
        xmlReader.Load(ConfigLoader.SETTINGS_FILE_NAME);

        XmlNodeList xmlNodeList = xmlReader.SelectNodes("Config/TextAlign");
        var TextAlign = (ConfigLoader.TextAlignEnum)Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/TextSize");
        var TextSize = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/MarginSize");
        var TextMargin = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/TextOffsetHorizontalFromCenter");
        var TextOffsetHorizontal = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/OnTop");
        var OnTop = Convert.ToBoolean(xmlNodeList[0].InnerText);
        var TextColor = new Vector3(Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/R")[0].InnerText),
            Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/G")[0].InnerText),
            Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/B")[0].InnerText));

        Console.WriteLine(
            $"### Loaded config with following settings ###" + 
            $"\nText align: {TextAlign.ToString()}" + 
            $"\nText size: {TextSize}" +
            $"\nText margin: {TextMargin}" + 
            $"\nText offset from center(horizontal): {TextOffsetHorizontal}" + 
            $" \nWrite text on top: {OnTop.ToString()}" +
            $" \nText color: {TextColor.ToString()}\n\n");
    }
    
    public override void SaveSettings()
    {
        throw new NotImplementedException();
    }
}