using System.Numerics;
using System.Xml;
using iTextSharp.text;

namespace WriteTextOnPdf.XML;

public static class XMLUtility
{
    public static int ReadInt(this XmlDocument xmlReader, int baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList== null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }
        var value = Convert.ToInt32(xmlNodeList[0].InnerText);
        baseValue = value;
        return baseValue;
    }
    
    public static float ReadFloat(this XmlDocument xmlReader, float baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList == null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }

        var value = Convert.ToSingle(xmlNodeList[0].InnerText);
        baseValue = value;
        return baseValue;
    }

    public static string ReadString(this XmlDocument xmlReader, string baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList == null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }

        var value = xmlNodeList[0].InnerText;
        baseValue = value;
        return baseValue;
    }
    
    public static bool ReadBool(this XmlDocument xmlReader, bool baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList == null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }

        var value = Convert.ToBoolean(xmlNodeList[0].InnerText);
        baseValue = value;
        return baseValue;
    }
    
    public static Vector2 ReadVector2(this XmlDocument xmlReader, Vector2 baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList == null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }

        var x = Convert.ToSingle(xmlNodeList[0].SelectNodes("X")[0].InnerText);
        var y = Convert.ToSingle(xmlNodeList[0].SelectNodes("Y")[0].InnerText);
        baseValue = new Vector2(x, y);
        return baseValue;
    }
    
    public static BaseColor ReadBaseColor(this XmlDocument xmlReader, BaseColor baseValue, string node)
    {
        XmlNodeList xmlNodeList = xmlReader.SelectNodes($"{node}");
        if (xmlNodeList == null || xmlNodeList.Count == 0)
        {
            return baseValue;
        }

        var r = Convert.ToInt32(xmlNodeList[0].SelectNodes("R")[0].InnerText);
        var g = Convert.ToInt32(xmlNodeList[0].SelectNodes("G")[0].InnerText);
        var b = Convert.ToInt32(xmlNodeList[0].SelectNodes("B")[0].InnerText);
        baseValue = new BaseColor(r, g, b);
        return baseValue;
    }
}