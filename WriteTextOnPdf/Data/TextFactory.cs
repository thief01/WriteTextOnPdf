namespace WriteTextOnPdf.Data;

public static class TextFactory
{
    public static List<PhraseData> GetPhasesFromText(string text)
    {
        var phrases = new List<PhraseData>();
        var textParts = text.Split('\n');
        for (var i = 0; i < textParts.Length; i++)
        {
            var textPart = textParts[i];
            var phrase = new PhraseData($"Text{i}");
            phrase.Phrase.Add(new ChunkData(textPart, phrase.NormalFont).GetChunk());
            phrases.Add(phrase);
        }

        return phrases;
    }
}