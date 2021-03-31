using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using YesWeDo.DataTableCollections;
using YesWeDo.DataTables;
using YesWeDo.MSBT;
using static YesweDo.Enums;
using static YesweDo.Extensions;

namespace YesweDo.Helpers
{
    class FileHelper
    {
        static Config config = new Config();
        const string defaultFileName = "Select Folder";

        public static void SaveRandomized(BattleDataOptions battleData, FighterDataOptions fighterData, int seed = -1, int iteration = 0)
        {
            // Do multiple randomizers, in case an impossible battle happens.  
            var fileName = config.file_name_encr;
            var randomizerString = iteration > 0 ? ".Randomizer " : "Randomizer ";
            var directory = config.file_directory_randomized + randomizerString + seed + " " + iteration;  // Randomizer ### 1

            Save(battleData, fighterData, directory, fileName, unencrypted:false, useFolderStructure: true, saveSpiritTitles:false);

            CopyPreloadFiles(directory);
            CopySpiritImages(directory);
        }

        public static void Save(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName, SpiritDataOptions spiritData = null, bool unencrypted = true, bool encrypted = true, bool useFolderStructure = false, bool saveSpiritTitles = true)
        {
            fileLocation += @"\";

            if (unencrypted)
            {
                var pathMod = useFolderStructure ? "" : config.unencr_sub;
                SaveUnencrypted(battleData, fighterData, fileLocation + pathMod, fileName);
            }
            if (encrypted)
            {
                SaveEncrypted(battleData, fighterData, fileLocation, fileName, useFolderStructure);
                if (spiritData?.HasData() ?? false)
                {
                    var loc = MiscDbsToSave();
                    SaveEncrypted(spiritData, Path.GetDirectoryName(loc), Path.GetFileName(loc));
                }
            }

            if (saveSpiritTitles)
            {
                SaveSpiritTitles(battleData.GetBattles(), config.file_directory_preload);
            }
        }

        // Replace Save calls to calls here.  
        public static void SaveUnencrypted(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName)
        {
            fileLocation = FixFolderEndPath(fileLocation);

            // Save the version for local editing. 
            Directory.CreateDirectory(fileLocation);

            SaveToFile(ConvertDataToXDocument(battleData, fighterData), fileLocation + fileName);
        }
        public static void SaveUnencrypted(SpiritDataOptions spiritData, string fileLocation, string fileName)
        {
            fileLocation = FixFolderEndPath(fileLocation);

            // Save the version for local editing. 
            Directory.CreateDirectory(fileLocation);

            SaveToFile(ConvertDataToXDocument(spiritData), fileLocation + fileName);
        }

        public static void SaveEncrypted(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string fileName, bool useFolderStructure = false)
        {
            fileLocation = FixFolderEndPath(fileLocation);

            // Save an encrypted version for direct placement on SD card. 
            if (useFolderStructure)
            {
                fileLocation += GetFilePath(fileName);
            }
            Directory.CreateDirectory(fileLocation);
            SaveToEncryptedFile(ConvertDataToXDocument(battleData, fighterData), fileLocation + fileName);
        }
        public static void SaveEncrypted(SpiritDataOptions spiritData, string fileLocation, string fileName)
        {
            fileLocation = FixFolderEndPath(fileLocation);

            Directory.CreateDirectory(fileLocation);
            SaveToEncryptedFile(ConvertDataToXDocument(spiritData), fileLocation + fileName);
        }

        public static XDocument ConvertDataToXDocument(BattleDataOptions battleData, FighterDataOptions fighterData)
        {
            return XmlHelper.BuildXml(battleData, fighterData);
        }
        public static XDocument ConvertDataToXDocument(SpiritDataOptions spiritData)
        {
            return XmlHelper.BuildXml(spiritData);
        }

        public static void SaveToEncryptedFile(XDocument doc, string fileLocation)
        {
            PrcCrypto encryptedSaver = new PrcCrypto();
            encryptedSaver.AssmebleEncrypted(doc.ToXmlDocument(), fileLocation, config.labels_file_location);
        }

        public static void SaveToFile(XDocument doc, string fileLocation)
        {
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
                File.Copy(file.FullName, dest + fileName, true);
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
                    var name = file.Name;
                    if (file.Name.Contains(Defs.FILE_PATCH_PATTERN))
                    {
                        foreach(var replacement in Defs.fileNameReplacements)
                        {
                            definedLocation = definedLocation.Replace(replacement.Item1, replacement.Item2);
                        }
                        name = name.Replace(Defs.FILE_PATCH_PATTERN, "");
                    }
                    errorMessage.Append(CopyFile(file, dest + definedLocation, name));
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
            foreach(var file in Defs.filesToCopy)
            {
                if (file.Item1.Equals(fileName)){
                    return file.Item2;
                }
            }
            return "";
        }

        public static bool FileExists(string fileName)
        {
            return new FileInfo(fileName ?? "").Exists;
        }
        public static string GetFilePathFromLikeName(string fileName)
        {
            foreach (var file in Defs.regExFiles)
            {
                if (Regex.IsMatch(fileName, file.Item1))
                {
                    return file.Item2;
                }
            }
            return "";
        }

        public static void CreateDirectories(List<string> dirs)
        {
            foreach (var dir in dirs)
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static CommonSaveFileDialog GetCommonSaveFolderDialog()
        {
            var dialog = new CommonSaveFileDialog()
            {
                DefaultFileName = "Select Folder",
                EnsureFileExists = false,
                EnsurePathExists = false
            };

            return dialog;
        }

        public static CommonOpenFileDialog GetImportBattleFileDialog(string title = null, string initialDirectory = null)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Title = title ?? dialog.Title;
            dialog.InitialDirectory = initialDirectory ?? dialog.InitialDirectory;

            foreach (var filter in EnumUtil<Import_Filters>.GetValues())
            {
                dialog.Filters.Add(new CommonFileDialogFilter(filter, $"*{filter}"));
            }

            return dialog;
        }

        public static bool IsDefaultFolderDialogPath(string filename)
        {
            return UiHelper.GetLastFolder(filename).Equals(defaultFileName);
        }

        public static string ToDefaultBattleExportFolder(string filename)
        {
            return UiHelper.GetParentFolder(filename);
        }

        public static void ExportStandalone(DataTbls dataTbls, string filename)
        {
            var selectedBattleId = dataTbls.selectedBattle.battle_id;

            ExportedBattle export = new ExportedBattle()
            {
                battle = dataTbls.battleData.GetBattle(selectedBattleId),
                fighters = dataTbls.selectedFighters,
                spirit = dataTbls?.spiritData?.GetSpiritByName(selectedBattleId)
            };

            var unencryptedFileName = String.Format("{0}{1}", selectedBattleId, Defs.standaloneExportExtension);

            // Check whether user entered custom folder name.  If not, use standard format.  
            var standalonePath = FileHelper.IsDefaultFolderDialogPath(filename) ?
                FileHelper.ToDefaultBattleExportFolder(filename) :
                filename;

            XmlHelper.SerializeToFile($"{standalonePath}\\{unencryptedFileName}", export);
        }

        public static void ExportPackaged(DataTbls dataTbls, string filename)
        {
            var selectedBattleId = dataTbls.selectedBattle.battle_id;

            // Check whether user entered custom folder name.  If not, use standard format.  
            var packPath = FileHelper.IsDefaultFolderDialogPath(filename) ?
                FileHelper.ToDefaultBattleExportFolder(filename) + @"\" + selectedBattleId + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") :
                filename;

            FileHelper.Save(dataTbls.battleData, dataTbls.fighterData, packPath, dataTbls.config.file_name_encr, useFolderStructure: true, unencrypted : false, encrypted : true, spiritData: dataTbls.spiritData, saveSpiritTitles: true);
            FileHelper.CopyPreloadFiles(packPath);
            FileHelper.CopySpiritImages(packPath);
        }

        public static bool SpiritImageMatchesBattle(FileInfo file, string battle_id)
        {
            return file.Name.Contains(battle_id);
        }

        public static string FixFolderEndPath(string filePath)
        {
            if (filePath[filePath.Length-1] != '\\')
            {
                return filePath + '\\';
            }

            return filePath;
        }

        public static string MiscDbsToSave()
        {
            foreach(var filesToSave in Defs.dbFilesToSave)
            {
                foreach(var preloadFile in FileHelper.GetFiles(config.file_directory_preload))
                {
                    if(preloadFile.Name == filesToSave)
                    {
                        return preloadFile.FullName;
                    }
                }
            }
            return null;
        }


        public static MsbtAdapter GetLoadedMsbtAdapter(string fileName)
        {
            var adapter = new MsbtAdapter();
            try
            {
                adapter.Load(fileName);
            }
            catch   // If it fails to load, adapter will be new, empty adapter.   If it doesn't, it will be filled.  either way, its what we want.  
            {
            }

            return adapter;
        }

        public static string OpenMsbtWithFilename(IEnumerable<Battle> battles, string fileName)
        {
            var adapter = GetLoadedMsbtAdapter(fileName);

            foreach (var battle in battles)
            {
                battle.SetSpiritTitleParameters(adapter.HashEntries.FirstOrDefault(x => x?.SpiritBattleId == battle?.battle_id)?.EditedText);
            }

            if (adapter != null && adapter.Entries.Count() > 0)
            {
                return "Spirit Titles";
            }
            else
            {
                return null;
            }
        }

        public static void SaveSpiritTitles(IEnumerable<Battle> battles, string fileName)
        {
            MsbtAdapter adapter;
            MsbtAdapter backupAdapter;
            try
            {
                var files = GetFiles(fileName);

                foreach(var file in files.Where( x=> Defs.msbtFilesToSave.Contains(x.Name)))
                {
                    var originalSize = file.Length;
                    adapter = GetLoadedMsbtAdapter(file.FullName);
                    backupAdapter = GetLoadedMsbtAdapter(file.FullName);
                    var updated = false;

                    foreach (var battle in battles.Where(x => x.msbtUpdated))
                    {
                        updated = true;
                        var match = adapter.Entries.FirstOrDefault(x => ((MsbtEntry)x).SpiritBattleId == battle.battle_id);
                        if (match != null)
                        {
                            match.EditedText = battle.combinedMsbtTitle;
                        }
                    }
                    if (updated)
                    {
                        adapter.Save();

                        // See if the file broke.  If so, write the old version back and throw exception.  
                        file.Refresh();
                        if(file.Length < originalSize / 2)
                        {
                            backupAdapter.Save();
                            throw new Exception("Error trying to save spirit titles.  Spirit Title changes unsaved.");
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
            }
        }
    }
}
