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
            pdfWriter.DirectContent.AddTemplate(page, settings.PdfScale.X, 0, 0, settings.PdfScale.Y,  settings.PdfOffset.X/* size.Width * 0.1f / 2*/, settings.PdfOffset.Y);

            var cb = pdfWriter.DirectContent;

            cb.SetColorFill(settings.TextColorScaled);
            cb.SetFontAndSize(settings.BaseFont, settings.TextSize);
            cb.BeginText();
            cb.ShowTextAligned((int)settings.TextAlign, parsedArgs.Text, settings.TextPosition.X,
                settings.TextPosition.Y /* size.Height - settings.TextSize */,
                0);
            cb.EndText();

            doc.Close();
            pdfWriter.Close();
        }

        private static (bool isOk, Args args) ValidateArgs(string[] args)
        {
            if (args.Length < 3)
            {
                // Console.WriteLine("Args needed: InputPath, OutputPath, TextToWrite");
                throw new Exception("\"Args needed: InputPath, OutputPath, TextToWrite\"");
                // return (false, null);
            }

            Args parsedArgs = new Args(args);

            return (true, parsedArgs);
        }
        
        private static void CheckInputPdf(string inputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new Exception($"File \"{inputPath}\" doesn't exist.");
                Console.WriteLine($"File \"{inputPath}\" doesn't exist.");
                return;
            }
        }
        
        private static (PdfWriter pdfWriter, PdfReader pdfReader, Document doc, Settings settings) CreateObjects(Args parsedArgs)
        {
            Settings settings = new Settings();
            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);

            var pdfReader = new PdfReader(sr);
            var size = pdfReader.GetPageSizeWithRotation(1);
            settings.CalculatePositions(size);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            return (pdfWriter, pdfReader, doc, settings);
        }
    }
}
