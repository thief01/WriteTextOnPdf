using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DodawanieTekstuDoPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            // Test();
            if (args.Length < 2)
            {
                Console.WriteLine("InputPath, OutputPath, TextToWrite, TextAlign");
                Console.WriteLine("- Text align, 0 - left, 1 center, 2 - right");
                return;
            }
            string sciezkaDoPlikuWejsciowego = args[0];
            string sciezkaDoPlikuWyjsciowego = args[1];

            if (!File.Exists(sciezkaDoPlikuWejsciowego))
            {
                Console.WriteLine($"Plik {sciezkaDoPlikuWejsciowego} nie istnieje.");
                return;
            }
            
            var sr = new FileStream(sciezkaDoPlikuWejsciowego, FileMode.Open);
            var fs = new FileStream(sciezkaDoPlikuWyjsciowego, FileMode.Create);
            
            var pdfReader = new PdfReader(sr);
            var pdfStamper = new PdfStamper(pdfReader, fs);
            var contentByte = pdfStamper.GetOverContent(1); // Wybieramy pierwszą stronę PDF

            // Tworzymy obiekt do renderowania tekstu
            var cb = pdfStamper.GetOverContent(1);
            
            // Tworzymy czcionkę
            // var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            
            BaseFont bf = BaseFont.CreateFont("ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
            //Font f = new iTextSharp.text.Font(bf, 12, Font.NORMAL, BaseColor.BLACK);

            // Ustawiamy czcionkę, kolor i rozmiar tekstu
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 12);

            // Tworzymy tekst, który chcemy dodać
            string tekst = "To jest nowy tekst dodany do istniejącego PDF ĄĘŹŻĆŁŃÓ.";

            // Ustalamy pozycję tekstu na stronie
            cb.BeginText();
            cb.ShowTextAligned(Element.ALIGN_LEFT, tekst, 100, 700, 0);
            cb.EndText();

            pdfStamper.Close();
            
            
        }
    }
}
