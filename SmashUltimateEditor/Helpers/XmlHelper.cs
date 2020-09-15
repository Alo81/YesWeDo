using paracobNET;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SmashUltimateEditor.Helpers
{
    class XmlHelper
    {
        static ParamFile file { get; set; }
        static XmlDocument xml { get; set; }

        public static void ReadUntilName(XmlReader reader, string stopper = "")
        {
            while (!reader.EOF && reader.Name != stopper)
            {
                reader.Read();
            }
        }

        public static void ReadUntilNodeType(XmlReader reader, XmlNodeType node = XmlNodeType.Element)
        {
            while (!reader.EOF && reader.NodeType != node)
            {
                reader.Read();
            }
        }
        public static void ReadUntilAttribute(XmlReader reader, string attribute)
        {
            while (!reader.EOF && reader.GetAttribute(attribute) is null)
            {
                reader.Read();
            }
        }

        public static DataOptions ReadXML(string fileName)
        {
            bool parseData = false;
            bool firstPass = false;
            bool sharedXmlName;
            IDataTbl dataTable;
            var results = new DataOptions();

            // Copy the stream to memory, so we're not holding the resource open.  
            MemoryStream stream = new MemoryStream();

            using (Stream fileStream = new FileStream(fileName, FileMode.Open))
            {
                fileStream.CopyTo(stream);
                stream.Position = 0;
            }

            XmlReader reader = XmlReader.Create(stream);

            try
            {
                reader.Read();
                // Read the whole file.  
                while (!reader.EOF)
                {
                    dataTable = DataTbl.GetDataTblFromXmlName(reader?.GetAttribute("hash"));

                    if (dataTable != null)
                    {
                        parseData = true;
                        firstPass = true;
                        sharedXmlName = dataTable.GetType() == typeof(DataTbl);

                        if (sharedXmlName)
                        {
                            MemoryStream firstLevelCopy = new MemoryStream();
                            // Store the position, read from the beginning, restore the position.
                            var position = stream.Position;
                            stream.Position = 0;
                            dataTable = DataTbl.DetermineXmlTypeFromFirstLevel(stream);
                            stream.Position = position;
                        }
                    }

                    if (parseData)
                    {
                        // Read until start of data.  
                        XmlHelper.ReadUntilName(reader, stopper: "struct");

                        // Lists have index values, so keep reading until we've left the list.  
                        // Added first pass to handle when a struct is not in a list.  
                        while (firstPass || reader.GetAttribute("index") != null)
                        {
                            dataTable = (IDataTbl)Activator.CreateInstance(dataTable.GetType());
                            dataTable.BuildFromXml(reader);

                            results.AddDataTbl(dataTable);

                            XmlHelper.ReadUntilNodeType(reader, node: XmlNodeType.Element);

                            firstPass = false;
                        }

                        //Left the list.  Don't parse the next lines.  
                        parseData = false;
                        Console.WriteLine("{0} Table Complete.", dataTable.GetType().ToString());

                        continue;
                    }

                    reader.Read();
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static XDocument BuildXml(BattleDataOptions battleData, FighterDataOptions fighterData)
        {

            var type = fighterData.GetFighters().GetType().Name.ToLower();
            type = type.Remove(type.Length - 2);

            XDocument doc =
                new XDocument(new XDeclaration("1.0", "utf-8", null),
                    new XElement("struct",
                        // <list hash="battle_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type,
                        new XAttribute("hash", battleData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            battleData.GetBattles().Select(battle =>
                            battle.GetAsXElement(battleData.GetBattleIndex(battle)
                            )
                            )
                        ),
                        // <list hash="fighter_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type,
                        new XAttribute("hash", fighterData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            fighterData.GetFighters().Select(fighter =>
                            fighter.GetAsXElement(fighterData.GetFighterIndex(fighter)
                            )
                            )
                        )
                    )
                );

            return doc;
        }

        public static void WriteXmlToFile(string fileName, XDocument xmlDoc)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            writer.Write(xmlDoc.Declaration.ToString() + "\r\n" + xmlDoc.ToString());
            writer.Close();
            writer.Dispose();
        }

        /*
        public XmlDocument BuildXml(DataOptions opt)
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

        public XmlNode StructToNode()
        {

            return null;
        }
        public XmlNode ListToNode()
        {

            return null;
        }
        public XmlNode ValueToNode()
        {

            return null;
        }

        void AssmebleEncrypted(DataOptions options)
        {
            var XmlNode = Param2Node(options);

            //file.Save(fileLocation);
        }

        static XmlNode Param2Node(IXmlType param)
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

        static XmlNode ParamStruct2Node(DataOptions structure)
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

        static XmlNode ParamArray2Node(ParamList array)
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

        static XmlNode ParamValue2Node(ParamValue value)
        {
            XmlNode xmlNode = xml.CreateElement(value.TypeKey.ToString());
            XmlText text = xml.CreateTextNode(value.ToString(hashToStringLabels));
            xmlNode.AppendChild(text);
            return xmlNode;
        }
        */
    }
}
