﻿using System.ComponentModel;
using System.Text;
using System.Xml;

namespace IotDataStation.Util
{
    internal static class XmlUtils
    {
        public static XmlWriterSettings XmlWriterSettings(Encoding encoding = null)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            
            settings.Encoding = encoding ?? Encoding.GetEncoding("utf-8");
            return settings;
        }

        public static string GetXmlAttributeValue(XmlNode node, string name, string defaultValue = "", bool toFirstCharUpper = false)
        {
            string resString;
            defaultValue = defaultValue ?? "";
            if (node?.Attributes == null)
            {
                resString = defaultValue;
            }
            else
            {
                resString = node.Attributes[name] != null ? node.Attributes[name].Value : defaultValue;
            }


            if (toFirstCharUpper)
            {
                resString = StringUtils.ToFirstCharUpper(resString);
            }
            return resString;
        }
        
        public static T GetXmlAttributeTypeValue<T>(XmlNode node, string name, T defaultValue = default(T))
        {
            string resString;

            if (node?.Attributes == null)
            {
                return defaultValue;
            }
            else
            {
                if (node.Attributes[name] != null)
                {
                    resString = node.Attributes[name].Value;
                }
                else
                {
                    return defaultValue;
                }
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));



                return (T)converter.ConvertFromString(resString);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int GetXmlAttributeIntValue(XmlNode node, string name, int defaultValue = 0)
        {
            int newResult = 0;

            string stringValue = GetXmlAttributeValue(node, name, defaultValue.ToString(), false);

            newResult = !int.TryParse(stringValue, out newResult) ? defaultValue : 0;

            return newResult;
        }

        public static double GetXmlAttributeDoubleValue(XmlNode node, string name, double defaultValue = 0.0d)
        {
            double newResult = 0.0d;

            string stringValue = GetXmlAttributeValue(node, name, defaultValue.ToString(), false);

            newResult = !double.TryParse(stringValue, out newResult) ? defaultValue : 0.0d;

            return newResult;
        }

        public static int CompareXmlAttributeValueTo(XmlNode node, string name, string compareValue, bool ignoreCase = false)
        {
            if (node == null)
            {
                return 0;
            }

            string attributeValue = "";
            string tempValue = "";

            if (ignoreCase)
            {
                attributeValue = GetXmlAttributeValue(node, name).ToLower();
                tempValue = compareValue.ToLower();
            }
            else
            {
                attributeValue = GetXmlAttributeValue(node, name);
                tempValue = compareValue;
            }
            return System.String.Compare(attributeValue, tempValue, System.StringComparison.Ordinal);
        }

        public static string GetXmlNodeInnerText(XmlNode node, string xpath="", string defaultValue = "")
        {
            string resString = defaultValue;

            XmlNode selectedNode = node;
            if (!string.IsNullOrWhiteSpace(xpath))
            {
                selectedNode = node?.SelectSingleNode(xpath);
            }
            if (selectedNode != null)
            {
                resString = selectedNode.InnerText.Trim();
            }
            return resString;
        }

        public static T GetXmlNodeInnerValue<T>(XmlNode node, string xpath, T defaultValue = default(T))
        {
            string foundData = GetXmlNodeInnerText(node, xpath);
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                return (T)converter.ConvertFromString(foundData);
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetXmlNodeInnerText(XmlDocument doc, string xpath, XmlNamespaceManager manager, string defaultValue = "")
        {
            string resString = defaultValue;
            XmlNode selectedNode = doc.SelectSingleNode(xpath, manager);
            if (selectedNode != null)
            {
                resString = selectedNode.InnerText.Trim();
            }
            return resString;
        }


        public static double GetXmlNodeInnerDoubleValue(XmlNode node, string xpath, double defaultValue = 0.0d)
        {
            double newResult = 0.0d;

            string stringValue = GetXmlNodeInnerText(node, xpath);

            newResult = !double.TryParse(stringValue, out newResult) ? defaultValue : 0.0d;

            return newResult;
        }

        public static int GetXmlNodeInnerIntValue(XmlNode node, string xpath, int defaultValue = 0)
        {
            int newResult = 0;

            string stringValue = GetXmlNodeInnerText(node, xpath);

            newResult = !int.TryParse(stringValue, out newResult) ? defaultValue : 0;

            return newResult;
        }

    }
}
