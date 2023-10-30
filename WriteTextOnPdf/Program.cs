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

            public Args(string[] args)
            {
                InputPath = args[0];
                OutputPath = args[1];
                Text = args[2];
                TextAlign = Convert.ToInt32(args[3]);
            }
        }
        static void Main(string[] args)
        {
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            if (args.Length < 4)
            {
                Console.WriteLine("InputPath, OutputPath, TextToWrite, TextAlign");
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
            
            // var fs2 = new FileStream("2_" + parsedArgs.OutputPath, FileMode.Create);
            // var pdfStamper = new PdfStamper(pdfReader, fs2);
            // var page2 = pdfStamper.GetImportedPage(pdfReader, 1);
            // var content = pdfStamper.GetOverContent(1);
            // content.AddTemplate(page2, 0.9f, 0, 0, 0.9f, 0, 0);
            // pdfStamper.Close();

            var size = pdfReader.GetPageSizeWithRotation(1);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            
            PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, 1);
            pdfWriter.DirectContent.AddTemplate(page, 0.9f,0, 0, 0.9f, size.Width*0.1f/2, 0);
            
            var cb = pdfWriter.DirectContent;

            BaseFont bf = BaseFont.CreateFont("ARIALUNI.TTF", BaseFont.IDENTITY_H, true);


            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 12);
            cb.BeginText();
            cb.ShowTextAligned(parsedArgs.TextAlign, parsedArgs.Text, size.Width/2, size.Height - 12, 0);
            cb.EndText();
            
            doc.Close();
            pdfWriter.Close();
        }
    }
}
