using SmashUltimateEditor.DataTableCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
            Save(battleData, fighterData, directory, fileName, unencrypted:false, useFolderStructure: true);

            CopyPreloadFiles(directory);
            CopySpiritImages(directory);
        }

        public static void Save(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName, bool unencrypted = true, bool encrypted = true, bool useFolderStructure = false)
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
                if (useFolderStructure)
                {
                    fileLocation += GetFilePath(fileName);
                }
                Directory.CreateDirectory(fileLocation);
                SaveToEncryptedFile(battleData, fighterData, fileLocation + fileName);
            }
        }

        // Replace Save calls to calls here.  
        public static void SaveEncrypted(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName, bool randomized = false)
        {
            fileLocation += @"\";

            // Save an encrypted version for direct placement on SD card. 
            if (randomized)
            {
                CopyPreloadFiles(fileLocation);
                fileLocation += GetFilePath(fileName);
            }
            Directory.CreateDirectory(fileLocation);
            SaveToEncryptedFile(battleData, fighterData, fileLocation + fileName);
        }

        public static void SaveUnencrypted(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName)
        {
            fileLocation += @"\";

            // Save the version for local editing. 
            var unencrLoc = fileLocation + config.unencr_sub + @"\";
            Directory.CreateDirectory(unencrLoc);
            SaveToFile(battleData, fighterData, unencrLoc + fileName);
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

        public static IEnumerable<FileInfo> GetFiles(string file_location)
        {
            try
            {
                return new DirectoryInfo(file_location).GetFiles();
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return null;
            }
        }
        public static IEnumerable<FileInfo> GetFilesMatching(string file_location, string match)
        {
            try
            {
                var files = new DirectoryInfo(file_location).GetFiles();
                var matchFiles = new List<FileInfo>();
                foreach(var file in files)
                {
                    if(SpiritImageMatchesBattle(file, match))
                    {
                        matchFiles.Add(file);
                    }
                }

                return matchFiles;
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return null;
            }
        }

        public static string CopyFile(string source, string dest, string fileName)
        {
            try
            {
                FileInfo file = new FileInfo(source);
                CopyFile(file, dest, fileName);
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n";
            }
            return "";
        }
        public static string CopyFile(FileInfo file, string dest, string fileName)
        {
            try
            {
                Directory.CreateDirectory(dest);
                File.Copy(file.FullName, dest + fileName);
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n";
            }
            return "";
        }

        public static void CopyFiles(IEnumerable<FileInfo> files, string dest)
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (var file in files)
            {
                var definedLocation = GetFilePath(file.Name);
                if (!String.IsNullOrWhiteSpace(definedLocation))
                {
                    errorMessage.Append(CopyFile(file, dest + definedLocation, file.Name));
                }
            }

            if (!String.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                UiHelper.PopUpMessage(errorMessage.ToString());
            }
        }

        public static void CopyFilesWithRegexMatch(IEnumerable<FileInfo> files, string dest)
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (var file in files)
            {
                var definedLocation = GetFilePathFromLikeName(file.Name);
                if (!String.IsNullOrWhiteSpace(definedLocation))
                {
                    errorMessage.Append(CopyFile(file, dest + definedLocation, file.Name));
                }
            }

            if (!String.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                UiHelper.PopUpMessage(errorMessage.ToString());
            }
        }

        public static void CopyPreloadFiles(string fileLocation)
        {
            CopyFiles(GetFiles(config.file_directory_preload), fileLocation);
        }
        public static void CopySpiritImages(string fileLocation)
        {
            CopyFilesWithRegexMatch(GetFiles(config.file_directory_spirit_images), fileLocation);
        }
        public static void CopySpiritImagesForBattle(string fileLocation, string battle_id)
        {
            CopyFilesWithRegexMatch(GetFilesMatching(config.file_directory_spirit_images, battle_id), fileLocation);
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
        public static string GetFilePathFromLikeName(string fileName)
        {
            foreach (var file in Defs.spiritUiLocations)
            {
                if (Regex.IsMatch(fileName, file.Item1))
                {
                    return file.Item2;
                }
            }
            return "";
        }

        public static bool SpiritImageMatchesBattle(FileInfo file, string battle_id)
        {
            return file.Name.Contains(battle_id);
        }
    }
}
