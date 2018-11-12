﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IotDataStation.Common.Interface;
using IotDataStation.Common.Util;
using IotDataStation.Common.Util;
using Newtonsoft.Json.Linq;
using NLog;

namespace IotDataStation.Common.DataModel
{
    public class Node : NodeBaseImpl, INode
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool IsNullNode => string.IsNullOrWhiteSpace(Id);

        public Node(string id, string name = "", NodeStatus status = NodeStatus.None, string groupName = "", NodePoint point = null, NodeAttributes attributes = null, NodeItems items = null, DateTime? updatedTime = null, string className = "") : base(id, name, status, groupName, point, attributes, items, updatedTime)
        {
            if (!string.IsNullOrWhiteSpace(className))
            {
                ClassName = className;
            }
        }

        public static INode CreateNullNode()
        {
            return new Node("", "", NodeStatus.None);
        }

        public static Node CreateFrom(INode iNode)
        {
            if (iNode == null)
            {
                return null;
            }
            return new Node(iNode.Id, iNode.Name, iNode.Status, iNode.GroupName, iNode.Point, iNode.Attributes, iNode.Items, iNode.UpdatedTime, iNode.ClassName);
        }

        public static Node CreateFrom(JObject nodeObject)
        {
            Node node = null;
            if (nodeObject == null)
            {
                return node;
            }

            try
            {
                string id = JsonUtils.GetStringValue(nodeObject, "id");

                if (string.IsNullOrWhiteSpace(id))
                {
                    Logger.Error("Cannot create Node, id is empty.");
                    return null;
                }
                string name = "";
                string className = "";
                NodeStatus status = NodeStatus.None;
                string groupName = "";
                DateTime updatedTime = CachedDateTime.Now;
                NodePoint point = null;
                NodeAttributes attributes = null;
                NodeItems items = null;


                foreach (var property in nodeObject.Properties())
                {
                    switch (property.Name)
                    {
                        case "id":
                            break;
                        case "name":
                            name = property.Value.Value<string>();
                            break;
                        case "class":
                            className = property.Value.Value<string>();
                            break;
                        case "status":
                            status = StringUtils.ToEnum(property.Value.Value<string>(), NodeStatus.None);
                            break;
                        case "group":
                            groupName = property.Value.Value<string>();
                            break;
                        case "updatedTime":
                            updatedTime = DateTime.ParseExact(property.Value.Value<string>(), "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
                            break;
                        case "point":
                            point = NodePoint.CreateFrom((JObject)nodeObject["point"]);
                            break;
                        case "items":
                            items = NodeItems.CreateFrom((JArray)nodeObject["items"]);
                            break;
                        default:
                            if (attributes == null)
                            {
                                attributes = new NodeAttributes();
                            }
                            attributes[property.Name] = property.Value.Value<string>();
                            break;
                    }
                }
                node = new Node(id, name, status, groupName, point, attributes, items, updatedTime);
                if (!string.IsNullOrWhiteSpace(className))
                {
                    node.ClassName = className.Trim();
                }
            }
            catch (Exception e)
            {
                node = null;
                Logger.Error(e, "CreateFrom(nodeObject):");
            }

            return node;
        }

    }
}