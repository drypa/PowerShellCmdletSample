using System;
using System.Configuration;
using System.Reflection;

namespace PowerShellCmdletSample
{
    internal class AppSettingsHelper
    {
        public static string ProxyDomain()
        {
            return GetSettingValue("ProxyDomain");
        }

        public static string ProxyPassword()
        {
            return GetSettingValue("ProxyPassword");
        }

        public static string ProxyUrl()
        {
            return GetSettingValue("ProxyUrl");
        }

        public static string ProxyUser()
        {
            return GetSettingValue("ProxyUser");
        }

        public static bool UseProxy()
        {
            KeyValueConfigurationCollection settings =
                ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings;

            string setting = GetSettingValue("UseProxy");
            if (string.IsNullOrEmpty(setting))
            {
                return false;
            }
            bool result;
            return bool.TryParse(setting, out result) && result;
        }

        private static string GetSettingValue(string settingKey)
        {
            KeyValueConfigurationCollection settings =
                ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings;

            KeyValueConfigurationElement setting = settings[settingKey];
            return setting == null ? null : setting.Value;
        }
    }
}
