﻿using paracobNET;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace YesweDo.Helpers
{
    class PrcCrypto
    {
        #region PRC Cryptography

        ParamFile file { get; set; }
        XmlDocument xml { get; set; }
        string labelName { get; set; }
        static OrderedDictionary<ulong, string> hashToStringLabels { get; set; }
        static OrderedDictionary<string, ulong> stringToHashLabels { get; set; }

        public XmlDocument DisassembleEncrypted(string fileLocation, string labelsFileLocation = "")
        {
            labelName = labelsFileLocation;

            if (!string.IsNullOrEmpty(labelName))
            {
                try
                {
                    if (hashToStringLabels == null)
                    {
                        hashToStringLabels = LabelIO.GetHashStringDict(labelName);
                        hashToStringLabels.Add(0x07c9e447b6, "");
                    }
                }
                catch (Exception ex)
                {
                    UiHelper.PopUpMessage(ex.Message);
                    return new XmlDocument();
                }
            }

            file = new ParamFile();
            file.Open(fileLocation);

            xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            xml.AppendChild(ParamStruct2Node(file.Root));

            return xml;
        }

        public void AssmebleEncrypted(XmlDocument doc, string fileLocation, string labelsFileLocation)
        {
            stringToHashLabels = new OrderedDictionary<string, ulong>();
            if (!string.IsNullOrEmpty(labelsFileLocation))
            {
                try
                {
                    stringToHashLabels = LabelIO.GetStringHashDict(labelsFileLocation);
                    stringToHashLabels.Add("", 0x07c9e447b6);
                }
                catch (Exception ex)
                {
                    UiHelper.PopUpMessage(ex.Message);
                    return;
                }
            }

            file = new ParamFile(Node2ParamStruct(doc.DocumentElement));

            file.Save(fileLocation);
        }

        XmlNode Param2Node(IParam param)
        {
            switch (param.TypeKey)
            {
                case ParamType.@struct:
                    return ParamStruct2Node(param as ParamStruct);
                case ParamType.list:
                    return ParamArray2Node(param as ParamList);
                default:
                    return ParamValue2Node(param as ParamValue);
            }
        }

        XmlNode ParamStruct2Node(ParamStruct structure)
        {
            XmlNode xmlNode = xml.CreateElement(ParamType.@struct.ToString());
            foreach (var node in structure.Nodes)
            {
                XmlNode childNode = Param2Node(node.Value);
                XmlAttribute attr = xml.CreateAttribute("hash");
                attr.Value = Hash40Util.FormatToString(node.Key, hashToStringLabels);
                childNode.Attributes.Append(attr);
                xmlNode.AppendChild(childNode);
            }
            return xmlNode;
        }

        XmlNode ParamArray2Node(ParamList array)
        {
            XmlNode xmlNode = xml.CreateElement(ParamType.list.ToString());
            XmlAttribute mainAttr = xml.CreateAttribute("size");
            mainAttr.Value = array.Nodes.Count.ToString();
            xmlNode.Attributes.Append(mainAttr);
            for (int i = 0; i < array.Nodes.Count; i++)
            {
                XmlNode childNode = Param2Node(array.Nodes[i]);
                XmlAttribute attr = xml.CreateAttribute("index");
                attr.Value = i.ToString();
                childNode.Attributes.Append(attr);
                xmlNode.AppendChild(childNode);
            }
            return xmlNode;
        }

        XmlNode ParamValue2Node(ParamValue value)
        {
            XmlNode xmlNode = xml.CreateElement(value.TypeKey.ToString());
            XmlText text = xml.CreateTextNode(value.ToString(hashToStringLabels));
            xmlNode.AppendChild(text);
            return xmlNode;
        }

        IParam Node2Param(XmlNode node)
        {
            try
            {
                if (!Enum.IsDefined(typeof(ParamType), node.Name))
                {
                    var x = "";
                    //throw new FormatException($"\"{node.Name}\" is not a valid param type");
                }
                ParamType type = (ParamType)Enum.Parse(typeof(ParamType), node.Name);
                switch (type)
                {
                    case ParamType.@struct:
                        return Node2ParamStruct(node);
                    case ParamType.list:
                        return Node2ParamArray(node);
                    default:
                        return Node2ParamValue(node, type);
                }
            }
            catch (Exception e)
            {
                //recursively add param node context to exceptions until we exit
                string trace = "Trace: " + node.Name;
                foreach (XmlAttribute attr in node.Attributes)
                    trace += $" ({attr.Name}=\"{attr.Value}\")";
                string message = trace + Environment.NewLine + e.Message;
                if (e.InnerException == null)
                    throw new Exception(message, e);
                else
                    throw new Exception(message, e.InnerException);
            }
        }

        ParamStruct Node2ParamStruct(XmlNode node)
        {
            Hash40Pairs<IParam> childParams = new Hash40Pairs<IParam>();
            foreach (XmlNode child in node.ChildNodes) {
                if (child.Attributes["hash"].Value == "fighter_kind")
                {
                    var x = "";
                }
                childParams.Add(child.Attributes["hash"].Value, stringToHashLabels, Node2Param(child));
            }
            return new ParamStruct(childParams);
        }

        ParamList Node2ParamArray(XmlNode node)
        {
            int count = node.ChildNodes.Count;
            List<IParam> children = new List<IParam>(count);
            for (int i = 0; i < count; i++)
                children.Add(Node2Param(node.ChildNodes[i]));
            return new ParamList(children);
        }

        ParamValue Node2ParamValue(XmlNode node, ParamType type)
        {
            ParamValue param = new ParamValue(type);
            if(node.InnerText == "jeann")
            {
                var x = "";
            }
            param.SetValue(node.InnerText, stringToHashLabels);
            return param;
        }

        enum BuildMode
        {
            Disassemble,
            Assemble,
            Invalid
        }
        #endregion
    }
}
