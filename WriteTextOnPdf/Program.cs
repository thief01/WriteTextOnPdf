using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EasyAddTextToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            (bool isOk, Args parsedArgs) = ValidateArgs(args);
            if(!isOk)
                return;
            
            CheckInputPdf(parsedArgs.InputPath);

            var (pdfWriter, pdfReader, doc, settings) = CreateObjects(parsedArgs);
            
            doc.Open();

            PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, 1);
            pdfWriter.DirectContent.AddTemplate(page, settings.PdfScale.X, 0, 0, settings.PdfScale.Y,  settings.PdfOffset.X, settings.PdfOffset.Y);

            var splitedText = GetTextLines(parsedArgs.Text);
            AddSimpleText(splitedText, pdfWriter, settings);

            doc.Close();
            pdfWriter.Close();
        }

        private static (bool isOk, Args args) ValidateArgs(string[] args)
        {
            if (args.Length < 3)
            {
                throw new Exception("\"Args needed: InputPath, OutputPath, TextToWrite\"");
            }

            Args parsedArgs = new Args(args);
            return (true, parsedArgs);
        }
        
        private static void CheckInputPdf(string inputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new Exception($"File \"{inputPath}\" doesn't exist.");
            }
        }
        
        private static (PdfWriter pdfWriter, PdfReader pdfReader, Document doc, Settings settings) CreateObjects(Args parsedArgs)
        {
            Settings settings = new Settings();
            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);

            var pdfReader = new PdfReader(sr);
            var size = pdfReader.GetPageSizeWithRotation(1);
            settings.CalculatePositions(size, GetTextLines(parsedArgs.Text).Length);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            return (pdfWriter, pdfReader, doc, settings);
        }
        
        private static void AddSimpleText(string[] text, PdfWriter pdfWriter, Settings settings)
        {
            var cb = pdfWriter.DirectContent;

            cb.SetColorFill(settings.TextColorScaled);
            cb.SetFontAndSize(settings.BaseFont, settings.TextSize);
            cb.BeginText();
            for (int i = 0; i < text.Length; i++)
            {
                Chunk chunk = new Chunk("test bold inject", new Font(Font.FontFamily.HELVETICA, settings.TextSize*2, Font.BOLD, new BaseColor(255,0,0, 255)));
                Chunk chunk2 = new Chunk("another one", new Font(Font.FontFamily.HELVETICA, settings.TextSize, Font.NORMAL, settings.TextColorScaled));
                Phrase phrase = new Phrase(text[i],
                    new Font(Font.FontFamily.HELVETICA, settings.TextSize, Font.NORMAL, settings.TextColorScaled));
                phrase.Add(chunk);
                phrase.Add(chunk2);
                // cb.ShowTextAligned((int)settings.TextAlign, text[i], settings.TextPosition.X, settings.TextPosition.Y + settings.LineOffset * i, 0);
                ColumnText.ShowTextAligned(cb, (int)settings.TextAlign, phrase, settings.TextPosition.X, settings.TextPosition.Y + settings.LineOffset * i, 0);
            }
           
            cb.EndText();
        }
        
        private static string[] GetTextLines(string text)
        {
            return text.Split("\\n");
        }
    }
}
