using System.Numerics;
using System.Text.RegularExpressions;
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
    private int lineId = 0;
    
    public PhraseData(string textName)
    {
        lineId = 0;
        Phrase = new Phrase();
        TextName = textName;
        LoadSettings();
        CreateFonts();
        // CreateOffsetPositionLine();
    }

    public PhraseData(string textName, PhraseData phraseData, int lineId = 0)
    {
        Phrase = new Phrase();
        
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
        TextName = textName;
        LoadSettings();
        CreateFonts();
    }

    public List<Chunk> CreateChunk(string text)
    {
        var splited = text.Split("<b>");
        List<(bool, string)> texts = new List<(bool, string)>();

        for (int i = 0; i < splited.Length; i++)
        {
            Console.WriteLine(splited[i]);
            if (splited[i].Contains("</b>"))
            {
                var splited2 = splited[i].Split("</b>");
                texts.Add(new ValueTuple<bool, string>(true, splited2[0]));
                texts.Add(new ValueTuple<bool, string>(false, splited2[1]));
            }
            else
            {
                texts.Add(new ValueTuple<bool, string>(false, splited[i]));
            }
        }

        if (texts.Count == 0)
        {
            texts.Add(new ValueTuple<bool, string>(false, text));
        }

        for (int i = 0; i < texts.Count; i++)
        {
            var chunk = new Chunk(texts[i].Item2, new Font(NormalFont, TextSize, texts[i].Item1 ? Font.BOLD : Font.NORMAL, TextColor));
            Phrase.Add(chunk);
        }
        return Phrase.Chunks.ToList();
    }

    public void LoadSettings()
    {
        var xmlDocument = ConfigLoader.XmlDocument;
        TextPosition = xmlDocument.ReadVector2(TextPosition, $"Config/{TextName}/TextPosition");
        // TextPosition = xmlDocument.ReadVector2($"Config/{TextName}/TextPosition");
        TextPivot = xmlDocument.ReadVector2(TextPivot, $"Config/{TextName}/TextPivot");
        TextColor = xmlDocument.ReadBaseColor(TextColor, $"Config/{TextName}/TextColor");
        baseFontName = xmlDocument.ReadString(baseFontName, $"Config/{TextName}/NormalFont");
        boldFontName = xmlDocument.ReadString(baseFontName, $"Config/{TextName}/BoldFont");
        TextSize = xmlDocument.ReadFloat(TextSize, $"Config/{TextName}/TextSize");
        OnTop = xmlDocument.ReadBool(OnTop, $"Config/{TextName}/OnTop");
        TextAlign = xmlDocument.ReadInt(TextAlign, $"Config/{TextName}/TextAlign");
    }

    private void CreateOffsetPositionLine()
    {
        TextPosition = new Vector2(TextPosition.X, TextPosition.Y + 15 * lineId);
    }
    
    private void CreateFonts()
    {
        NormalFont = BaseFont.CreateFont(baseFontName, BaseFont.IDENTITY_H, true);
        BoldFont = BaseFont.CreateFont(boldFontName, BaseFont.IDENTITY_H, true);
    }
}