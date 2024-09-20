using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using static CommonTools.Enums.CommonEnums;

namespace CommonTools
{
	public static class ConfigManager
	{
		public static bool ConfigExists(string configName, ConfigTypes configType)
		{
			if (string.IsNullOrWhiteSpace(configName))
			{
				throw new ArgumentNullException(nameof(configName));
			}
			var configExists = false;
			switch (configType)
			{
				case ConfigTypes.ConnectionString:
					try
					{
						var config = ConfigurationManager.ConnectionStrings[configName];
						configExists = config != null;
					}
					catch (ConfigurationErrorsException)
					{
						configExists = false;
					}

					break;
				case ConfigTypes.AppSettings:
					configExists = ConfigurationManager.AppSettings.AllKeys!.Contains(configName);
					break;
				default:
					break;
			}
			return configExists;
		}

		public static string GetAppConfig(string configName)
		{
			if (!ConfigExists(configName, ConfigTypes.AppSettings))
			{
				return default;
			}
			return ConfigurationManager.AppSettings[configName];
		}

		public static void SetAppConfig(string configName, string configValue)
		{
			if (string.IsNullOrWhiteSpace(configName))
			{
				throw new ArgumentNullException(nameof(configName));
			}
			try
			{
				var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				if (ConfigExists(configName, ConfigTypes.AppSettings))
				{
					config.AppSettings.Settings[configName].Value = configValue;
				}
				else
				{
					var newConfig = new KeyValueConfigurationElement(configName, configValue);
					config.AppSettings.Settings.Add(newConfig);
				}
				config.Save(ConfigurationSaveMode.Modified);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static string GetConnectionString(string connectionName)
		{
			if (!ConfigExists(connectionName, ConfigTypes.ConnectionString))
			{
				return default;
			}
			return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
		}

		public static void SetConnectionString(string connectionName, string connectionString, string provideName)
		{
			if (string.IsNullOrWhiteSpace(connectionName))
			{
				throw new ArgumentNullException(nameof(connectionName));
			}
			try
			{
				var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				if (ConfigExists(connectionName, ConfigTypes.ConnectionString))
				{
					config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString = connectionString;
					config.ConnectionStrings.ConnectionStrings[connectionName].ProviderName = provideName;
				}
				else
				{
					var newConfig = new ConnectionStringSettings(connectionName, connectionString, provideName);
					config.ConnectionStrings.ConnectionStrings.Add(newConfig);
				}
				config.Save(ConfigurationSaveMode.Modified);
			}
			catch (Exception)
			{
				throw;
			}

		}

	}
}
