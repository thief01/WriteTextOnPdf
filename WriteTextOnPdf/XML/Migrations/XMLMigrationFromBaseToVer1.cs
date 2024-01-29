using System.Numerics;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WriteTextOnPdf.XML.Migrations;

public class XMLMigrationFromBaseToVer1 : XMLMigrationBase
{
    private enum TextAlignEnum
    {
        left,
        center,
        right
    }

    private TextAlignEnum TextAlign { get; set; } = TextAlignEnum.center;
    private int TextSize { get; set; } = 18;
    private int TextMargin { get; set; } = 20;
    private int TextOffsetHorizontal { get; set; } = 0;
    private bool OnTop { get; set; } = true;
    private Vector3 TextColor { get; set; } = new Vector3(0, 0, 0);
    
    public override string FromVersion => "base";
    public override string ToVersion => "1.0";
    
    public override void MigrateSettings()
    {
        // Console.WriteLine("You have OLD XMLversion but migration system not added yet.");
        // Console.ReadKey();
        // Console.ReadKey();
        // throw new NotImplementedException();
        // LoadSettings();
        // SaveSettings();
    }
    
    /// <summary>
    /// Todo: migration logic
    /// </summary>
    public override void LoadSettings()
    {
        XmlDocument xmlReader = ConfigLoader.XmlDocument;

        XmlNodeList xmlNodeList = xmlReader.SelectNodes("Config/TextAlign");
        TextAlign = (TextAlignEnum)Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/TextSize");
        TextSize = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/MarginSize");
        TextMargin = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/TextOffsetHorizontalFromCenter");
        TextOffsetHorizontal = Convert.ToInt32(xmlNodeList[0].InnerText);
        xmlNodeList = xmlReader.SelectNodes("Config/OnTop");
        OnTop = Convert.ToBoolean(xmlNodeList[0].InnerText);
        TextColor = new Vector3(120, 120, 120);
        

        Console.WriteLine(
            $"### Loaded config with following settings ###" + 
            $"\nText align: {TextAlign.ToString()}" + 
            $"\nText size: {TextSize}" +
            $"\nText margin: {TextMargin}" + 
            $"\nText offset from center(horizontal): {TextOffsetHorizontal}" + 
            $" \nWrite text on top: {OnTop.ToString()}" +
            $" \nText color: {TextColor.ToString()}\n\n");
    }
    
    public override void SaveSettings()
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement root = xmlDocument.CreateElement("Config");
        xmlDocument.AppendChild(root);
        
        var xmlVersion = xmlDocument.CreateElement("XMLVersion");
        xmlVersion.InnerText = "1.0";
        root.AppendChild(xmlVersion);
        CreatePdfSettings(xmlDocument, root);
        CreateTextPhaseGlobal(xmlDocument, root);
        
        xmlDocument.Save(ConfigLoader.Instance.LoadedConfig);
        ConfigLoader.Instance.ReloadSettings();
    }

    private void CreatePdfSettings(XmlDocument xmlDocument, XmlElement root)
    {
        XmlElement pdfSettings = xmlDocument.CreateElement("PdfSettings");
        root.AppendChild(pdfSettings);
        
        XmlElement xmlAutoDetecSpace = xmlDocument.CreateElement("AutoDetecSpace");
        xmlAutoDetecSpace.InnerText = "true";
        pdfSettings.AppendChild(xmlAutoDetecSpace);
        
        XmlElement xmlTopSpace = xmlDocument.CreateElement("TopSpace");
        xmlTopSpace.InnerText = "0";
        pdfSettings.AppendChild(xmlTopSpace);
        
        XmlElement xmlBotSpace = xmlDocument.CreateElement("BottomSpace");
        xmlBotSpace.InnerText = "0";
        pdfSettings.AppendChild(xmlBotSpace);
        
        XmlElement xmlPdfScale = xmlDocument.CreateElement("PdfScale");
        pdfSettings.AppendChild(xmlPdfScale);
        
        XmlElement xmlPdfScaleX = xmlDocument.CreateElement("X");
        xmlPdfScaleX.InnerText = "0.9";
        xmlPdfScale.AppendChild(xmlPdfScaleX);
        
        XmlElement xmlPdfScaleY = xmlDocument.CreateElement("Y");
        xmlPdfScaleY.InnerText = "0.9";
        xmlPdfScale.AppendChild(xmlPdfScaleY);
        
        XmlElement xmlPdfOffset = xmlDocument.CreateElement("PdfOffset");
        pdfSettings.AppendChild(xmlPdfOffset);
        
        XmlElement xmlPdfOffsetX = xmlDocument.CreateElement("X");
        xmlPdfOffsetX.InnerText = "0";
        xmlPdfOffset.AppendChild(xmlPdfOffsetX);
        
        XmlElement xmlPdfOffsetY = xmlDocument.CreateElement("Y");
        xmlPdfOffsetY.InnerText = "0";
        xmlPdfOffset.AppendChild(xmlPdfOffsetY);
    }

    private void CreateTextPhaseGlobal(XmlDocument xmlDocument, XmlElement root, string textNode="GlobalTextPhase")
    {
        XmlElement GlobalTextPhase = xmlDocument.CreateElement(textNode);
        root.AppendChild(GlobalTextPhase);
        
        XmlElement pivotValue = xmlDocument.CreateElement("TextPivot");
        GlobalTextPhase.AppendChild(pivotValue);
        
        XmlElement xmlTextPivotXValue = xmlDocument.CreateElement("X");
        xmlTextPivotXValue.InnerText = "0.5";
        pivotValue.AppendChild(xmlTextPivotXValue);
        
        XmlElement xmlTextPivotYValue = xmlDocument.CreateElement("Y");
        xmlTextPivotYValue.InnerText = "0.5";
        pivotValue.AppendChild(xmlTextPivotYValue);
        
        XmlElement xmlTextAlignValue = xmlDocument.CreateElement("TextAlign");
        xmlTextAlignValue.InnerText = ((int)TextAlign).ToString();
        GlobalTextPhase.AppendChild(xmlTextAlignValue);
        
        XmlElement xmlTextPositionValue = xmlDocument.CreateElement("TextPosition");
        GlobalTextPhase.AppendChild(xmlTextPositionValue);
        
        XmlElement xmlTextPositionXValue = xmlDocument.CreateElement("X");
        xmlTextPositionXValue.InnerText = "0.5";
        xmlTextPositionValue.AppendChild(xmlTextPositionXValue);
        
        XmlElement xmlTextPositionYValue = xmlDocument.CreateElement("Y");
        xmlTextPositionYValue.InnerText = "0.96";
        xmlTextPositionValue.AppendChild(xmlTextPositionYValue);
        
        XmlElement xmlOnTopValue = xmlDocument.CreateElement("OnTop");
        xmlOnTopValue.InnerText = OnTop.ToString();
        GlobalTextPhase.AppendChild(xmlOnTopValue);
        
        XmlElement xmlTextColorValue = xmlDocument.CreateElement("TextColor");
        GlobalTextPhase.AppendChild(xmlTextColorValue);
        
        XmlElement xmlTextColorRValue = xmlDocument.CreateElement("R");
        xmlTextColorRValue.InnerText = TextColor.X.ToString();
        xmlTextColorValue.AppendChild(xmlTextColorRValue);
        
        XmlElement xmlTextColorGValue = xmlDocument.CreateElement("G");
        xmlTextColorGValue.InnerText = TextColor.Y.ToString();
        xmlTextColorValue.AppendChild(xmlTextColorGValue);
        
        XmlElement xmlTextColorBValue = xmlDocument.CreateElement("B");
        xmlTextColorBValue.InnerText = TextColor.Z.ToString();
        xmlTextColorValue.AppendChild(xmlTextColorBValue);
        
        XmlElement xmlTextSizeValue = xmlDocument.CreateElement("TextSize");
        xmlTextSizeValue.InnerText = TextSize.ToString();
        GlobalTextPhase.AppendChild(xmlTextSizeValue);
        
        XmlElement xmlTextFontValue = xmlDocument.CreateElement("NormalFont"); 
        xmlTextFontValue.InnerText = "font.ttf";
        GlobalTextPhase.AppendChild(xmlTextFontValue);
        
        XmlElement xmlTextBoldFontValue = xmlDocument.CreateElement("BoldFont");
        xmlTextBoldFontValue.InnerText = "font_B.ttf";
        GlobalTextPhase.AppendChild(xmlTextBoldFontValue);
    }
}