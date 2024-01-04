using System.Drawing;
using System.Numerics;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;

namespace EasyAddTextToPdf
{
    public class Settings
    {
        private const string SETTINGS_FILE_NAME = "config.xml";

        public enum TextAlignEnum
        {
            left,
            center,
            right
        }

        public TextAlignEnum TextAlign { get; private set; } = TextAlignEnum.center;
        public int TextSize { get; private set; } = 18;
        public int TextMargin { get; private set; } = 20;
        public int TextOffsetHorizontal { get; private set; } = 0;
        public bool OnTop { get; private set; } = true;
        public Vector2 PdfScale { get; private set; } = new Vector2(0.9f, 0.9f);
        public Vector2 PdfOffset { get; private set; } = new Vector2(0, 0);
        public Vector2 TextPosition { get; private set; } = new Vector2(0, 0);
        public BaseFont BaseFont { get; private set; }

        public BaseColor TextColorScaled
        {
            get
            {
                return new BaseColor(TextColor.X / 255f, TextColor.Y / 255f, TextColor.Z / 255f);
            }
        }

        
        private Vector3 TextColor { get; set; } = new Vector3(0, 0, 0);
        
        public Settings()
        {
            BaseFont = BaseFont.CreateFont("FONT.TTF", BaseFont.IDENTITY_H, true);
            if (File.Exists(SETTINGS_FILE_NAME))
                LoadSettings();
            else
            {
                Console.Write("Config doesn't exist.");
            }
        }

        public void CalculatePositions(Rectangle size)
        {
            float neededSize = TextSize + TextMargin;
            float calculatedScale = (size.Height - neededSize ) / size.Height;
            float invertedScale = 1 - calculatedScale;

            PdfScale = new Vector2(calculatedScale, calculatedScale);

            float textY = OnTop ? size.Height - TextSize - TextMargin / 2 : 0 + TextMargin / 2 + 3;
            
            PdfOffset = new Vector2(size.Width * invertedScale / 2, OnTop ? 0 : size.Height * invertedScale);

            TextPosition = new Vector2(size.Width / 2, textY);
        }

        private void LoadSettings()
        {
            XmlDocument xmlReader = new XmlDocument();
            xmlReader.Load(SETTINGS_FILE_NAME);

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
            TextColor = new Vector3(Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/R")[0].InnerText),
                Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/G")[0].InnerText),
                Convert.ToInt32(xmlReader.SelectNodes("Config/TextColor/B")[0].InnerText));

            Console.WriteLine(
                $"### Loaded config with following settings ###" + 
                $"\nText align: {TextAlign.ToString()}" + 
                $"\nText size: {TextSize}" +
                $"\nText margin: {TextMargin}" + 
                $"\nText offset from center(horizontal): {TextOffsetHorizontal}" + 
                $" \nWrite text on top: {OnTop.ToString()}" +
                $" \nText color: {TextColor.ToString()}\n\n");
        }
    }
}