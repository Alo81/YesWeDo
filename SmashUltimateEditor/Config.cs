using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SmashUltimateEditor
{
    public class Config
    {

        public string file_name;
        public string file_name_encr;

        public string file_directory;
        public string file_directory_custom_battles;
        public string file_directory_encr;
        public string file_directory_randomized;

        public string file_location;
        public string file_location_custom_battles;
        public string file_location_encr;

        public string labels_file_location;

        public bool encrypt;
        public bool decrypt;
        public int chaos;

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

            file_directory_custom_battles = file_directory + @"CustomBattles\";
            file_directory_encr = file_directory + @"Encrypted\";
            file_directory_randomized = file_directory + @"Randomized\";

            file_location = file_directory + file_name;
            file_location_custom_battles = file_directory_custom_battles + file_name;
            file_location_encr = file_directory_encr + file_name_encr;
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
                MessageBox.Show(String.Format("Could not set value from config: {0} for {1}.\r\n\r\n{2}", val, name, ex.Message));
                throw ex;
            }
        }
    }
}
