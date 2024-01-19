using System.Drawing;
using System.Numerics;
using System.Xml;
using WriteTextOnPdf.XML;

namespace WriteTextOnPdf.Data;

public class PdfSettings
{
    public const string STATIC_XML_PATH = "Config/PdfSettings/";
    public static PdfSettings Instance => _instance ??= new PdfSettings();
    private static PdfSettings _instance;
    public bool AutoDetecSpace { get; set; }
    public float TopSpace { get; set; }
    public float BotSpace { get; set; }
    public Vector2 PdfScale { get; set; }
    public Vector2 PdfOffset { get; set; }
    
    public PdfSettings()
    {
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        XmlDocument xmlDocument = ConfigLoader.XmlDocument;
        AutoDetecSpace = xmlDocument.ReadBool(AutoDetecSpace, $"{STATIC_XML_PATH}AutoDetecSpace");
        TopSpace = xmlDocument.ReadFloat(TopSpace, $"{STATIC_XML_PATH}TopSpace");
        BotSpace = xmlDocument.ReadFloat(BotSpace, $"{STATIC_XML_PATH}BotSpace");
        PdfScale = xmlDocument.ReadVector2(PdfScale, $"{STATIC_XML_PATH}PdfScale");
        PdfOffset = xmlDocument.ReadVector2(PdfOffset, $"{STATIC_XML_PATH}PdfOffset");
    }
    
    public Vector2 GetOffsetFromPageSize(Vector2 size)
    {
        var invertedScaleY = 1 - PdfScale.Y + PdfOffset.Y;
        var invertedScaleX = 1 - PdfScale.X + PdfOffset.X;
        float offsetX = size.X * invertedScaleX / 2;
        float offsetY = size.Y * invertedScaleY / 2;
        return new Vector2(offsetX, offsetY);
    }
}