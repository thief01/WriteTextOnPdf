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

            if (args.Length < 4)
            {
                Console.WriteLine("InputPath, OutputPath, TextToWrite, TextAlign");
                Console.WriteLine("- Text align, 0 - left, 1 center, 2 - right");
                return;
            }
            string inputPath = args[0];
            string outputPath = args[1];
            string text = args[2];
            int textAlign = Convert.ToInt32(args[3]);

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"File \"{inputPath}\" doesn't exist.");
                return;
            }
            
            var sr = new FileStream(inputPath, FileMode.Open);
            var fs = new FileStream(outputPath, FileMode.Create);
            
            var pdfReader = new PdfReader(sr);
            var pdfStamper = new PdfStamper(pdfReader, fs);

            var cb = pdfStamper.GetOverContent(1);

            BaseFont bf = BaseFont.CreateFont("ARIALUNI.TTF", BaseFont.IDENTITY_H, true);

            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 12);
            cb.BeginText();
            cb.ShowTextAligned(textAlign, text, 100, 700, 0);
            cb.EndText();

            pdfStamper.Close();
        }
    }
}
