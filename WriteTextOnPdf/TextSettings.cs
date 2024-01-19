using System.Numerics;
using System.Xml;
using System.Xml.Serialization;
using EasyAddTextToPdf.XML;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EasyAddTextToPdf;

public class TextSettings
{
    public string TextName { get; set; }
    public Vector2 TextPosition { get; set; }
    public Vector2 TextPivot { get; set; }
    public BaseColor TextColor { get; set; }
    public BaseFont BaseFont { get; set; }
    public float TextSize { get; set; }
    public bool OnTop { get; set; }
    
    private XmlDocument xmlReader;
    
    private string baseFontName;
    
    public TextSettings(TextSettings settings)
    {
        TextPosition = settings.TextPosition;
        TextPivot = settings.TextPivot;
        TextColor = settings.TextColor;
        BaseFont = settings.BaseFont;
        TextSize = settings.TextSize;
        OnTop = settings.OnTop;
        baseFontName = settings.baseFontName;
    }

    public TextSettings(string textName, XmlDocument xmlReader)
    {
        TextName = textName;
        this.xmlReader = xmlReader;
        LoadSettings();
        CreateFont();
    }
   
    public Chunk GetChunk(string text)
    {
        return new Chunk(text, new Font(BaseFont, TextSize, Font.NORMAL, TextColor));
    }

    private void LoadSettings()
    {
        TextPosition = xmlReader.ReadVector2(TextPosition, $"Config/Texts/{TextName}/TextPosition");
        TextPivot = xmlReader.ReadVector2(TextPivot, $"Config/Texts/{TextName}/TextPivot");
        TextColor = xmlReader.ReadBaseColor(TextColor, $"Config/Texts/{TextName}/TextColor");
        baseFontName = xmlReader.ReadString(baseFontName, $"Config/Texts/{TextName}/BaseFont");
        TextSize = xmlReader.ReadFloat(TextSize, $"Config/Texts/{TextName}/TextSize");
        OnTop = xmlReader.ReadBool(OnTop, $"Config/Texts/{TextName}/OnTop");
    }
    
    private void CreateFont()
    {
        BaseFont = BaseFont.CreateFont(baseFontName, BaseFont.IDENTITY_H, true);
    }
}