using System;
using System.Collections.Generic;
using System.Configuration;
using CommonTools.Enums;

namespace CommonTools
{
	public static class ConfigManager
	{
		private static readonly Dictionary<ConfigType, ConfigStrategy> _strategies = new Dictionary<ConfigType, ConfigStrategy>
		{
			{ ConfigType.AppSettings, new AppSettingsStrategy() },
			{ ConfigType.ConnectionString, new ConnectionStringStrategy() }
		};

		#region public methods
		public static bool ConfigExists(string configName, ConfigType configType)
		{
			if (string.IsNullOrWhiteSpace(configName))
			{
				throw new ArgumentNullException(nameof(configName));
			}
			return _strategies.TryGetValue(configType, out var strategy) && strategy.Exists(configName);
		}

		public static string GetAppConfig(string configName)
		{
			return _strategies[ConfigType.AppSettings].Get(configName);
		}

		public static void SetAppConfig(string configName, string configValue)
		{
			if (string.IsNullOrWhiteSpace(configName))
			{
				throw new ArgumentNullException(nameof(configName));
			}
			_strategies[ConfigType.AppSettings].Set(configName, configValue, null);
		}

		public static string GetConnectionString(string connectionName)
		{
			return _strategies[ConfigType.ConnectionString].Get(connectionName);
		}

		public static void SetConnectionString(string connectionName, string connectionString, string providerName = null)
		{
			if (string.IsNullOrWhiteSpace(connectionName))
			{
				throw new ArgumentNullException(nameof(connectionName));
			}
			_strategies[ConfigType.ConnectionString].Set(connectionName, connectionString, providerName);
		}

		#endregion

		#region Private Strategies

		private abstract class ConfigStrategy
		{
			internal abstract bool Exists(string name);
			internal abstract string Get(string name);
			internal abstract void Set(string name, string value, string extra);

			protected Configuration OpenConfig() =>
				ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		}

		private class AppSettingsStrategy : ConfigStrategy
		{
			internal override bool Exists(string name) => ConfigurationManager.AppSettings[name] != null;

			internal override string Get(string name) => ConfigurationManager.AppSettings[name];

			internal override void Set(string name, string value, string extra)
			{
				var config = OpenConfig();
				if (config.AppSettings.Settings[name] != null)
				{
					config.AppSettings.Settings[name].Value = value;
				}
				else
				{
					config.AppSettings.Settings.Add(name, value);
				}
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
		}

		private class ConnectionStringStrategy : ConfigStrategy
		{
			internal override bool Exists(string name) => ConfigurationManager.ConnectionStrings[name] != null;

			internal override string Get(string name) => ConfigurationManager.ConnectionStrings[name]?.ConnectionString;

			internal override void Set(string name, string value, string extra)
			{
				var config = OpenConfig();
				var settings = config.ConnectionStrings.ConnectionStrings[name];
				if (settings != null)
				{
					settings.ConnectionString = value;
					if (!string.IsNullOrWhiteSpace(extra)) settings.ProviderName = extra;
				}
				else
				{
					var newConfig = new ConnectionStringSettings(name, value);
					if (!string.IsNullOrWhiteSpace(extra)) newConfig.ProviderName = extra;
					config.ConnectionStrings.ConnectionStrings.Add(newConfig);
				}
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("connectionStrings");
			}
		}
		#endregion
	}
}
