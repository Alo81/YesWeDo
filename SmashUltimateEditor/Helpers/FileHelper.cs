using SmashUltimateEditor.DataTableCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmashUltimateEditor.Helpers
{
    class FileHelper
    {
        static Config config = new Config();
        public static void Save(BattleDataOptions battleData, FighterDataOptions fighterData)
        {
            Save(battleData, fighterData, Path.GetDirectoryName(config.file_location), Path.GetFileName(config.file_location));
        }

        public static void SaveRandomized(BattleDataOptions battleData, FighterDataOptions fighterData, int seed = -1, int iteration = 0)
        {
            // Do multiple randomizers, in case an impossible battle happens.  
            var fileName = config.file_name_encr;
            var directory = config.file_directory_randomized + "Randomizer " + seed + " " + iteration;  // Randomizer ### 1
            Save(battleData, fighterData, directory, fileName, unencrypted:false);
        }

        public static void Save(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName, bool unencrypted = true, bool encrypted = true)
        {
            fileLocation += @"\";

            if (unencrypted)
            {
                // Save the version for local editing. 
                var unencrLoc = fileLocation + config.unencr_sub + @"\";
                Directory.CreateDirectory(unencrLoc);
                SaveToFile(battleData, fighterData, unencrLoc + fileName);
            }
            if (encrypted)
            {
                // Save an encrypted version for direct placement on SD card. 
                CopyPreloadFiles(fileLocation);
                var modFileLocation = fileLocation + GetFilePath(fileName);
                Directory.CreateDirectory(modFileLocation);
                SaveToEncryptedFile(battleData, fighterData, modFileLocation + fileName);
            }
        }

        public static void SaveToEncryptedFile(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation)
        {
            var doc = XmlHelper.BuildXml(battleData, fighterData);

            PrcCrypto.AssmebleEncrypted(doc.ToXmlDocument(), fileLocation, config.labels_file_location);
        }

        public static void SaveToFile(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation)
        {
            var doc = XmlHelper.BuildXml(battleData, fighterData);
            XmlHelper.WriteXmlToFile(fileLocation, doc);
        }

        public static void CopyPreloadFiles(string fileLocation)
        {
            StringBuilder errorMessage = new StringBuilder();
            FileInfo[] files;
            try
            {
                var preLoad = config.file_directory_preload;
                files = new DirectoryInfo(preLoad).GetFiles();
            }
            catch(Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return;
            }

            foreach(var file in files)
            {
                var definedLocation = GetFilePath(file.Name);
                if (!String.IsNullOrWhiteSpace(definedLocation))
                {
                    try
                    {
                        var copyLocation = fileLocation + definedLocation;
                        Directory.CreateDirectory(copyLocation);
                        File.Copy(file.FullName, copyLocation + file.Name);
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Append(ex.Message + "\r\n");
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                UiHelper.PopUpMessage(errorMessage.ToString());
            }
        }

        public static string GetFilePath(string fileName)
        {
            foreach(var file in Defs.files)
            {
                if (file.Item1.Equals(fileName)){
                    return file.Item2;
                }
            }
            return "";
        }
    }
}
