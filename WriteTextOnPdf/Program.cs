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

            if (args.Length < 3)
            {
                Console.WriteLine("InputPath, OutputPath, TextToWrite");
                return;
            }

            Args parsedArgs = new Args(args);
            Settings settings = new Settings();
            
            if (!File.Exists(parsedArgs.InputPath))
            {
                Console.WriteLine($"File \"{parsedArgs.InputPath}\" doesn't exist.");
                return;
            }

            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);

            var pdfReader = new PdfReader(sr);

            var size = pdfReader.GetPageSizeWithRotation(1);
            settings.CalculatePositions(size);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, 1);
            pdfWriter.DirectContent.AddTemplate(page, settings.PdfScale.X, 0, 0, settings.PdfScale.Y,  settings.PdfOffset.X/* size.Width * 0.1f / 2*/, settings.PdfOffset.Y);

            var cb = pdfWriter.DirectContent;
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(settings.BaseFont, settings.TextSize);
            cb.BeginText();
            cb.ShowTextAligned((int)settings.TextAlign, parsedArgs.Text, settings.TextPosition.X,
                settings.TextPosition.Y /* size.Height - settings.TextSize */,
                0);
            cb.EndText();

            doc.Close();
            pdfWriter.Close();
        }
    }
}
