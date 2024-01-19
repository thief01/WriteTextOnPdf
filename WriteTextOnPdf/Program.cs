using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WriteTextOnPdf.UnitTests;

namespace WriteTextOnPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigLoader configLoader = new ConfigLoader();
            UnitTestManager unitTestManager = new UnitTestManager();
            return;
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
        
        private static (PdfWriter pdfWriter, PdfReader pdfReader, Document doc, ConfigLoader settings) CreateObjects(Args parsedArgs)
        {
            ConfigLoader configLoader = new ConfigLoader();
            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);

            var pdfReader = new PdfReader(sr);
            var size = pdfReader.GetPageSizeWithRotation(1);
            configLoader.CalculatePositions(size, GetTextLines(parsedArgs.Text).Length);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            return (pdfWriter, pdfReader, doc, configLoader);
        }
        
        private static void AddSimpleText(string[] text, PdfWriter pdfWriter, ConfigLoader configLoader)
        {
            var cb = pdfWriter.DirectContent;

            cb.SetColorFill(configLoader.TextColorScaled);
            cb.SetFontAndSize(configLoader.BaseFont, configLoader.TextSize);
            cb.BeginText();
            ColumnText ct = new ColumnText(cb);
            
            for (int i = 0; i < text.Length; i++)
            {
                Chunk chunk = new Chunk("test bold inject", new Font(Font.FontFamily.HELVETICA, configLoader.TextSize*2, Font.BOLD, new BaseColor(255,0,0, 255)));
                Chunk chunk2 = new Chunk("another one", new Font(Font.FontFamily.HELVETICA, configLoader.TextSize, Font.NORMAL, configLoader.TextColorScaled));
                Chunk n = new Chunk(Environment.NewLine);
                Phrase phrase = new Phrase(text[i],
                    new Font(Font.FontFamily.HELVETICA, configLoader.TextSize, Font.NORMAL, configLoader.TextColorScaled));
                ct.AddText(chunk);
                ct.AddText(chunk2);
                phrase.Add(chunk);
                phrase.Add(n);
                phrase.Add(chunk2);
                // cb.ShowTextAligned((int)settings.TextAlign, text[i], settings.TextPosition.X, settings.TextPosition.Y + settings.LineOffset * i, 0);
                ColumnText.ShowTextAligned(cb, (int)configLoader.TextAlign, phrase, configLoader.TextPosition.X, configLoader.TextPosition.Y + configLoader.LineOffset * i, 0);
            }
           
            cb.EndText();
        }
        
        private static string[] GetTextLines(string text)
        {
            return text.Split("\\n");
        }
    }
}
