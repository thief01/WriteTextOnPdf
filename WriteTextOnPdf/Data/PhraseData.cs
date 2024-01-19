using System.Numerics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WriteTextOnPdf.XML;

namespace WriteTextOnPdf.Data;

public class PhraseData
{
    public string TextName { get; set; }
    public Vector2 TextPosition { get; set; }
    public Vector2 TextPivot { get; set; }
    public BaseColor TextColor { get; set; }
    public BaseFont NormalFont { get; set; }
    public BaseFont BoldFont { get; set; }
    public float TextSize { get; set; }
    public int TextAlign { get; set; }
    public bool OnTop { get; set; }

    public Phrase Phrase { get; set; }
    
    private string baseFontName;
    private string boldFontName;
    
    public PhraseData(string textName)
    {
        Phrase = new Phrase();
        TextName = textName;
        LoadSettings();
        CreateFonts();
    }

    public PhraseData(string textName, PhraseData phraseData)
    {
        Phrase = new Phrase();
        TextName = textName;
        TextPosition = phraseData.TextPosition;
        TextPivot = phraseData.TextPivot;
        TextColor = phraseData.TextColor;
        NormalFont = phraseData.NormalFont;
        BoldFont = phraseData.BoldFont;
        TextSize = phraseData.TextSize;
        TextAlign = phraseData.TextAlign;
        OnTop = phraseData.OnTop;
        baseFontName = phraseData.baseFontName;
        boldFontName = phraseData.boldFontName;
        LoadSettings();
        CreateFonts();
    }

    public Chunk CreateChunk(string text)
    {
        var chunk =  new Chunk(text, new Font(NormalFont, TextSize, Font.NORMAL, TextColor));
        Phrase.Add(chunk);
        return chunk;
    }

    public void LoadSettings()
    {
        var xmlDocument = ConfigLoader.XmlDocument;
        TextPosition = xmlDocument.ReadVector2(TextPosition, $"Config/{TextName}/TextPosition");
        TextPivot = xmlDocument.ReadVector2(TextPivot, $"Config/{TextName}/TextPivot");
        TextColor = xmlDocument.ReadBaseColor(TextColor, $"Config/{TextName}/TextColor");
        baseFontName = xmlDocument.ReadString(baseFontName, $"Config/{TextName}/NormalFont");
        boldFontName = xmlDocument.ReadString(baseFontName, $"Config/{TextName}/BoldFont");
        TextSize = xmlDocument.ReadFloat(TextSize, $"Config/{TextName}/TextSize");
        OnTop = xmlDocument.ReadBool(OnTop, $"Config/{TextName}/OnTop");
        TextAlign = xmlDocument.ReadInt(TextAlign, $"Config/{TextName}/TextAlign");
    }
    
    private void CreateFonts()
    {
        NormalFont = BaseFont.CreateFont(baseFontName, BaseFont.IDENTITY_H, true);
        BoldFont = BaseFont.CreateFont(boldFontName, BaseFont.IDENTITY_H, true);
    }
}