using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using YesweDo.Helpers;

namespace YesweDo
{
    public class Config
    {

        public string file_directory_sub
        {
            get { return UiHelper.GetLastFolder(file_directory); }
        }
        public string custom_battles_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_custom_battles); }
        }
        public string encr_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_encr); }
        }
        public string unencr_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_unencr); }
        }
        public string randomized_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_randomized); }
        }
        public string preload_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_preload); }
        }
        public string spirit_images_sub
        {
            get { return UiHelper.GetLastFolder(file_directory_spirit_images); }
        }

        static string defaultFileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        const string defaultDbName = "ui_spirits_battle_db.prc";
        const string defaultEncrypted = @"Encrypted\";
        const string defaultCustomBattlesDirectory = @"CustomBattles\";
        const string defaultUnencrypted = @"Unencrypted\";
        const string defaultRandomized = @"Randomized\";
        const string defaultPreload = @"Preload\";
        const string defaultSpiritImages = @"Spirit Images\";
        const string defaultParamLabels = Defs.paramLabelsName;
        const int defaultChaosValue = 100;
        const int defaultRandomizerIterations = 3;

        public string file_name;
        public string file_name_encr;

        public string file_directory;
        public string file_directory_custom_battles;
        public string file_directory_encr;
        public string file_directory_unencr;
        public string file_directory_randomized;
        public string file_directory_preload;
        public string file_directory_spirit_images;

        public string file_location;
        public string file_location_encr;
        public string file_location_unencr;

        public bool check_for_updates;

        public string labels_file_location;
        public int chaos;
        public int randomizer_iterations;
        public int minimum_battle_time;
        public string excluded_fields;
        public IEnumerable<string> EXCLUDED_RANDOMIZED;

        public Config()
        {

            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        SetValueFromName(key, appSettings[key]);
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }

            file_name = String.IsNullOrEmpty(file_name) ? defaultDbName : file_name;
            file_name_encr = String.IsNullOrEmpty(file_name_encr) ? file_name : file_name_encr;

            file_directory = String.IsNullOrEmpty(file_directory) ? FileHelper.FixFolderEndPath(defaultFileDirectory) : file_directory;
            file_directory_custom_battles = String.IsNullOrEmpty(file_directory_custom_battles) ? file_directory + defaultCustomBattlesDirectory : file_directory_custom_battles;
            file_directory_encr = String.IsNullOrEmpty(file_directory_encr) ? file_directory + defaultEncrypted : file_directory_encr;
            file_directory_unencr = String.IsNullOrEmpty(file_directory_unencr) ? file_directory + defaultUnencrypted : file_directory_unencr;
            file_directory_randomized = String.IsNullOrEmpty(file_directory_randomized) ? file_directory + defaultRandomized : file_directory_randomized; 
            file_directory_preload = String.IsNullOrEmpty(file_directory_preload) ? file_directory + defaultPreload : file_directory_preload;
            file_directory_spirit_images = String.IsNullOrEmpty(file_directory_spirit_images) ? file_directory + defaultSpiritImages : file_directory_spirit_images;
            labels_file_location = String.IsNullOrEmpty(labels_file_location) ? file_directory + defaultParamLabels : labels_file_location;

            chaos = chaos == default(int) ? defaultChaosValue : chaos;
            randomizer_iterations = randomizer_iterations == default(int) ? defaultRandomizerIterations : randomizer_iterations;
            randomizer_iterations = randomizer_iterations < 1 ? 1 : randomizer_iterations;


            file_location = file_directory + file_name;
            file_location_encr = file_directory_encr + file_name_encr;
            file_location_unencr = file_directory_unencr + file_name;

            EXCLUDED_RANDOMIZED = SplitExcludedFields(excluded_fields??"");
        }

        public IEnumerable<string> SplitExcludedFields(string excluded)
        {
            return excluded.Split(Defs.CONFIG_DELIMITER);
        }

        public List<string> GetFileDirectories()
        {
            return new List<string>()
            {
                file_directory_custom_battles,
                file_directory_randomized,
                file_directory_preload,
                file_directory_spirit_images
            };
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public void SetValueFromName(string name, string val)
        {
            FieldInfo field = this.GetType().GetField(name);
            // If val is null, interpret as empty string for our purposes.  
            val ??= "";

            try
            {
                field.SetValue(this, Convert.ChangeType(val, field.FieldType));
            }
            catch (Exception ex)
            {
                try
                {
                    // Setting numeric value to empty string?  Use int default instead.  
                    field.SetValue(this, Convert.ChangeType("0", field.FieldType));
                }
                catch(Exception inEx)
                {
                    UiHelper.PopUpMessage(String.Format("Could not set value from config: {0} for {1}.\r\n\r\n{2}", val, name, inEx.Message));
                }
            }
        }
    }
}
