using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WriteTextOnPdf.Data;

public class ChunkData
{
    public string Text { get; set; }
    public BaseFont Font { get; set; }
    
    public ChunkData(string text, BaseFont font)
    {
        Text = text;
        Font = font;
    }

    public Chunk GetChunk()
    {
        return new Chunk(Text, new Font(Font));
    }
}