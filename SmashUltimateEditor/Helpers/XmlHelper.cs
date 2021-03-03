using paracobNET;
using SmashUltimateEditor.DataTables;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SmashUltimateEditor.Helpers
{
    class XmlHelper
    {
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

        public static Stream GetStreamFromFile(string fileName)
        {
            // Copy the stream to memory, so we're not holding the resource open.  
            MemoryStream stream = new MemoryStream();

            // Open file into stream.  
            try
            {
                using (Stream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                    stream.Position = 0;
                }

                return stream;
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return null;
            }
        }

        public static Stream GetStreamFromEncryptedFile(string fileName, string fileLocationLabels)
        {
            var stream = new MemoryStream();

            try
            {
                // Try opening again, and decrypting this time.
                XmlDocument doc = PrcCrypto.DisassembleEncrypted(fileName, fileLocationLabels);
                doc.Save(stream);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return null;
            }
        }

        public static XmlReader GetXmlReaderFromStream(Stream stream)
        {
            // Open stream into reader.  
            try
            {
                return  XmlReader.Create(stream);
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return null;
            }
        }

        public static DataOptions ReadXML(string fileName, string fileLocationLabels = "")
        {
            bool parseData = false;
            bool firstPass = false;
            bool sharedXmlName;
            IDataTbl dataTable;
            var results = new DataOptions();

            var stream = GetStreamFromFile(fileName);

            if (stream == null)
            {
                return results;
            }

            var reader = GetXmlReaderFromStream(stream);

            // Try to read.  If we can't read - it might be encrypted.  Try to decrypt.  
            try
            {
                reader.Read();
            }
            catch
            {
                try
                {
                    // Close and dispose reader and stream.
                    reader.Dispose();
                    stream = GetStreamFromEncryptedFile(fileName, fileLocationLabels);
                    reader = GetXmlReaderFromStream(stream);
                    reader.Read();
                }
                catch
                {
                    return results;
                }
            }

            // Read through entire file, and determine type.  
            try
            {
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
                            // Store the position, read from the beginning, restore the position.
                            var position = stream.Position;
                            stream.Position = 0;
                            dataTable = DataTbl.DetermineXmlTypeFromFirstLevel(stream);
                            stream.Position = position;

                            if(dataTable == null)
                            {
                                parseData = false;
                            }
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
            using StreamWriter writer = new StreamWriter(fileName, false);

            writer.Write(xmlDoc.Declaration.ToString() + "\r\n" + xmlDoc.ToString());
            writer.Close();
            writer.Dispose();
        }
    }
}
