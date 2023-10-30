using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace RozmiarStronyPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            string sciezkaDoPlikuPDF = "plik.pdf";

            if (System.IO.File.Exists(sciezkaDoPlikuPDF))
            {
                using (var sr = new System.IO.FileStream(sciezkaDoPlikuPDF, System.IO.FileMode.Open))
                {
                    var pdfReader = new PdfReader(sr);
                    int numerStrony = 1; // Numer strony, którą chcesz sprawdzić

                    if (numerStrony >= 1 && numerStrony <= pdfReader.NumberOfPages)
                    {
                        var pageSize = pdfReader.GetPageSize(numerStrony);
                        float szerokosc = pageSize.Width;
                        float wysokosc = pageSize.Height;

                        Console.WriteLine($"Rozmiar strony {numerStrony} w punktach:");
                        Console.WriteLine($"Szerokość: {szerokosc}pt");
                        Console.WriteLine($"Wysokość: {wysokosc}pt");
                    }
                    else
                    {
                        Console.WriteLine("Podano niepoprawny numer strony.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Plik PDF nie istnieje.");
            }
        }
    }
}