using WriteTextOnPdf.Data;
using WriteTextOnPdf.XML;

namespace WriteTextOnPdf.UnitTests;

public class UnitTestManager
{
    public UnitTestManager()
    {
        XmlUtilityReader();
        PdfSettingsTest();
        // var args = new Args(new[] {"", "", ""});
        // var phrases = TextFactory.GetPhasesFromText(args.Text);
        // pdfMaker.MakePdf(args.InputPath, args.OutputPath, phrases);
    }
    
    public void XmlUtilityReader()
    {
        var pdf = ConfigLoader.XmlDocument.ReadString("NON EXIST PATH", "NON_EXIST_PATH");
        // Console.WriteLine(pdf.PdfScale);
        Console.WriteLine("Xml test: passed");
        // if no errors then test passed
    }

    public void PdfSettingsTest()
    {
        var pdf = PdfSettings.Instance;
        // Console.WriteLine(pdf.PdfScale);
        Console.WriteLine("Pdf settings test: passed");
        // if no errors then test passed
    }
}