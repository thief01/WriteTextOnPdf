using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EasyAddTextToPdf
{
    class Program
    {
        private class Args
        {
            public string InputPath { get; }
            public string OutputPath { get; }
            public string Text { get; }
            public int TextAlign { get; }
            public int TextSize { get; }

            public Args(string[] args)
            {
                InputPath = args[0];
                OutputPath = args[1];
                Text = args[2];
                TextAlign = Convert.ToInt32(args[3]);
                TextSize = Convert.ToInt32(args[4]);
            }
        }
        static void Main(string[] args)
        {
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            if (args.Length < 4)
            {
                Console.WriteLine("InputPath, OutputPath, TextToWrite, TextAlign, TextSize");
                Console.WriteLine("- Text align, 0 - left, 1 center, 2 - right");
                return;
            }

            Args parsedArgs = new Args(args);
            
            if (!File.Exists(parsedArgs.InputPath))
            {
                Console.WriteLine($"File \"{parsedArgs.InputPath}\" doesn't exist.");
                return;
            }
            
            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);
            
            var pdfReader = new PdfReader(sr);
            
            var size = pdfReader.GetPageSizeWithRotation(1);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            
            PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, 1);
            pdfWriter.DirectContent.AddTemplate(page, 0.9f,0, 0, 0.9f, size.Width*0.1f/2, 0);
            
            var cb = pdfWriter.DirectContent;

            BaseFont bf = BaseFont.CreateFont("FONT.TTF", BaseFont.IDENTITY_H, true);


            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, parsedArgs.TextSize);
            cb.BeginText();
            cb.ShowTextAligned(parsedArgs.TextAlign, parsedArgs.Text, size.Width/2, size.Height - parsedArgs.TextSize, 0);
            cb.EndText();
            
            doc.Close();
            pdfWriter.Close();
        }
    }
}
