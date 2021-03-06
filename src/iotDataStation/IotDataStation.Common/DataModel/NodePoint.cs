﻿using System;
using System.Xml;
using IotDataStation.Common.Util;
using Newtonsoft.Json.Linq;
using NLog;

namespace IotDataStation.Common.DataModel
{
    public class NodePoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public double X { get; set; }
        public double Y { get; set; }

        public NodePoint(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }

        public JObject ToJObject()
        {
            JObject pointObject = new JObject();
            pointObject["x"] = X;
            pointObject["y"] = Y;
            return pointObject;
        }

        public JArray ToJArray()
        {
            JArray pointArray = new JArray();
            pointArray.Add(Y);
            pointArray.Add(X);
            return pointArray;
        }

        public void WriteXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Point");

            xmlWriter.WriteAttributeString("x", $"{X}");
            xmlWriter.WriteAttributeString("y", $"{Y}");

            xmlWriter.WriteEndElement();
        }


        public static NodePoint CreateFrom(JArray jArray)
        {
            if (jArray == null)
            {
                return null;
            }
            if (jArray.Count != 2)
            {
                return null;
            }

            NodePoint pointObject = null;
            try
            {
                double x = jArray[0].Value<double>();
                double y = jArray[1].Value<double>();
                pointObject = new NodePoint(x, y);
            }
            catch (Exception e)
            {
                pointObject = null;
                Logger.Error(e, "CreateFrom(JArray jArray):");
            }

            return pointObject;
        }

        public static NodePoint CreateFrom(JObject pointJObject)
        {
            if (pointJObject == null)
            {
                return null;
            }

            if (pointJObject.Type == JTokenType.Array)
            {
                return CreateFrom(((JToken)pointJObject) as JArray);
            }

            NodePoint pointObject = null;
            try
            {
                double x = JsonUtils.GetDoubleValue(pointJObject, "x");
                double y = JsonUtils.GetDoubleValue(pointJObject, "y");
                pointObject = new NodePoint(x, y);
            }
            catch (Exception e)
            {
                pointObject = null;
                Logger.Error(e, "CreateFrom(pointObject):");
            }
            return pointObject;
        }
        public static NodePoint CreateFrom(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                return null;
            }
            NodePoint pointObject = null;
            try
            {
                double x = XmlUtils.GetXmlAttributeDoubleValue(xmlNode, "x");
                double y = XmlUtils.GetXmlAttributeDoubleValue(xmlNode, "y");
                pointObject = new NodePoint(x, y);
            }
            catch (Exception e)
            {
                pointObject = null;
                Logger.Error(e, "CreateFrom(pointObject):");
            }
            return pointObject;
        }

        public NodePoint Clone()
        {
            return new NodePoint(X, Y);
        }
    }
}
