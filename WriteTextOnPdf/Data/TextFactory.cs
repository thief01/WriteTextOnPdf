using iTextSharp.text;

namespace WriteTextOnPdf.Data;

public static class TextFactory
{
    public static List<PhraseData> GetPhasesFromText(TextData textData)
    {
        var globalPhrase = new PhraseData("GlobalTextPhase");
        var lines = textData.Text.Split('\n');
        var phrases = new List<PhraseData>();

        for (int i = 0; i < lines.Length; i++)
        {
            PhraseData phraseData = new PhraseData(textData.TextName, globalPhrase, i);
            phraseData.CreateChunk(textData.Text);
            phrases.Add(phraseData);
        }

        return phrases;
    }
}