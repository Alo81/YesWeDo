using SmashUltimateEditor.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SmashUltimateEditor
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

        public string labels_file_location;
        public int chaos;
        public int randomizer_iterations;

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

            randomizer_iterations = randomizer_iterations < 1 ? 1 : randomizer_iterations;
            file_name = String.IsNullOrEmpty(file_name) ? @"ui_spirits_battle_db.prc" : file_name;
            file_name_encr = String.IsNullOrEmpty(file_name_encr) ? file_name : file_name_encr;
            file_directory_custom_battles = String.IsNullOrEmpty(file_directory_custom_battles) ? file_directory + @"CustomBattles\" : file_directory_custom_battles;
            file_directory_encr = String.IsNullOrEmpty(file_directory_encr) ? file_directory + @"Encrypted\" : file_directory_encr;
            file_directory_unencr = String.IsNullOrEmpty(file_directory_unencr) ? file_directory + @"Unencrypted\" : file_directory_unencr;
            file_directory_randomized = String.IsNullOrEmpty(file_directory_randomized) ? file_directory + @"Randomized\" : file_directory_randomized; 
            file_directory_preload = String.IsNullOrEmpty(file_directory_preload) ? file_directory + @"Preload\" : file_directory_preload;
            file_directory_spirit_images = String.IsNullOrEmpty(file_directory_spirit_images) ? file_directory + @"Spirit Images\" : file_directory_spirit_images;
            labels_file_location = String.IsNullOrEmpty(labels_file_location) ? file_directory + @"ParamLabels.csv" : labels_file_location;

            file_location = file_directory + file_name;
            file_location_encr = file_directory_encr + file_name_encr;
            file_location_unencr = file_directory_unencr + file_name;
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
                UiHelper.PopUpMessage(String.Format("Could not set value from config: {0} for {1}.\r\n\r\n{2}", val, name, ex.Message));
                throw ex;
            }
        }
    }
}
