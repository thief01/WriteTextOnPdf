using System.Numerics;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WriteTextOnPdf.Data;
using WriteTextOnPdf.UnitTests;

namespace WriteTextOnPdf
{
    class Program
    {
        private const bool USE_TEST = false;
        private static Vector2 pageSize;
        static void Main(string[] args)
        {
            if (USE_TEST)
            {
                UnitTestManager unitTestManager = new UnitTestManager();
            }
            
            new ConfigLoader();
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            (bool isOk, Args parsedArgs) = ValidateArgs(args);
            if(!isOk)
                return;
            
            CheckInputPdf(parsedArgs.InputPath);

            var (pdfWriter, pdfReader, doc) = CreateObjects(parsedArgs);
            
            doc.Open();

            var pdfSettings = PdfSettings.Instance;
            pageSize = new Vector2(pdfReader.GetPageSizeWithRotation(1).Width, pdfReader.GetPageSizeWithRotation(1).Height);
            var pageOffset = pdfSettings.GetOffsetFromPageSize(pageSize);
            var rotation = pdfReader.GetPageRotation(1) + 180;
            Console.WriteLine("PDF Rotation: " + rotation);
            PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, 1);
            if (rotation > 360)
            {
                rotation *= -1;
                rotation %= 360;
            }
            if(rotation == -90 || rotation == -270)
            {
                rotation *= -1;
                var radians = rotation * Math.PI / 180;
                var cos = Math.Cos(radians);
                var sin = Math.Sin(radians);
                var scale = pdfSettings.PdfScale;
                pdfWriter.DirectContent.AddTemplate(page, scale.X * cos, scale.X*sin, -scale.Y*sin, scale.Y*cos, pdfReader.GetPageSizeWithRotation(1).Width - pageOffset.X, pageOffset.Y);
            }
            else if (rotation == 90 || rotation == 270)
            {
                var radians = rotation * Math.PI / 180;
                var cos = Math.Cos(radians);
                var sin = Math.Sin(radians);
                var scale = pdfSettings.PdfScale;
                pdfWriter.DirectContent.AddTemplate(page, scale.X * cos, scale.X*sin, -scale.Y*sin, scale.Y*cos,  pageOffset.X, pdfReader.GetPageSizeWithRotation(1).Height - pageOffset.Y);
            }
            else
            {
                pdfWriter.DirectContent.AddTemplate(page, pdfSettings.PdfScale.X, 0, 0, pdfSettings.PdfScale.Y, pageOffset.X, pageOffset.Y);
            }
                
            
                
            //pdfWriter.DirectContent.AddTemplate(page, scale.X * cos, scale.X*sin, -scale.Y*sin, scale.Y*cos,  pageOffset.X, pdfReader.GetPageSizeWithRotation(1).Height - pageOffset.Y);
            
            // pdfWriter.DirectContent.AddTemplate(page, pdfSettings.PdfScale.X, 0, 0, pdfSettings.PdfScale.Y,
                // pageOffset.X, pageOffset.Y);

            AddTexts(pdfWriter, parsedArgs);

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
        
        private static (PdfWriter pdfWriter, PdfReader pdfReader, Document doc) CreateObjects(Args parsedArgs)
        {
            var sr = new FileStream(parsedArgs.InputPath, FileMode.Open);
            var fs = new FileStream(parsedArgs.OutputPath, FileMode.Create);

            var pdfReader = new PdfReader(sr);
            var size = pdfReader.GetPageSizeWithRotation(1);
            var doc = new Document(size);
            var pdfWriter = PdfWriter.GetInstance(doc, fs);

            return (pdfWriter, pdfReader, doc);
        }

        private static void AddTexts(PdfWriter pdfWriter, Args parsedArgs)
        {
            Console.WriteLine("Adding texts.");
            Console.WriteLine(pdfWriter == null);
            var cb = pdfWriter.DirectContent;
            cb.BeginText();
            
            for (int i = 0; i < parsedArgs.Texts.Count; i++)
            {
                var phraseDatas = TextFactory.GetPhasesFromText(parsedArgs.Texts[i]);
                for (int j = 0; j < phraseDatas.Count; j++)
                {
                    ColumnText.ShowTextAligned(cb, phraseDatas[j].TextAlign, phraseDatas[j].Phrase,
                        pageSize.X * phraseDatas[j].TextPosition.X, pageSize.Y * phraseDatas[j].TextPosition.Y, 0);
                }
            }
           
            cb.EndText();
        }
        
        private static string[] GetTextLines(string text)
        {
            return text.Split("\\n");
        }
    }
}
