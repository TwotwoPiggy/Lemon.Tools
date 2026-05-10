using System;
using System.Collections.Generic;
using System.Configuration;
using CommonTools.Enums;

namespace CommonTools
{
    /// <summary>
    /// 传统配置管理器，基于 System.Configuration。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>此类已过时，不推荐在新项目中使用。</b>
    /// </para>
    /// <para>
    /// 推荐替代方案：
    /// <list type="bullet">
    /// <item><description>使用 <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> 进行配置管理</description></item>
    /// <item><description>使用 <see cref="Configuration.IConnectionConfigProvider"/> 进行连接字符串管理</description></item>
    /// <item><description>使用 Options 模式进行强类型配置绑定</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// 迁移示例：
    /// <code>
    /// // 旧方式
    /// var connStr = ConfigManager.GetConnectionString("Default");
    /// 
    /// // 新方式（依赖注入）
    /// public class MyService
    /// {
    ///     private readonly IConfiguration _configuration;
    ///     public MyService(IConfiguration configuration) => _configuration = configuration;
    ///     
    ///     public void DoWork()
    ///     {
    ///         var connStr = _configuration.GetConnectionString("Default");
    ///     }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    [Obsolete("ConfigManager is deprecated. Use Microsoft.Extensions.Configuration.IConfiguration instead. See class documentation for migration guide.")]
    public static class ConfigManager
    {
        private static readonly Dictionary<ConfigType, ConfigStrategy> _strategies = new Dictionary<ConfigType, ConfigStrategy>
        {
            { ConfigType.AppSettings, new AppSettingsStrategy() },
            { ConfigType.ConnectionString, new ConnectionStringStrategy() }
        };

        /// <summary>
        /// 检查指定配置是否存在。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configType">配置类型。</param>
        /// <returns>如果配置存在返回 true，否则返回 false。</returns>
        [Obsolete("Use IConfiguration.GetSection() or IConfiguration.GetConnectionString() instead.")]
        public static bool ConfigExists(string configName, ConfigType configType)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                throw new ArgumentNullException(nameof(configName));
            }
            return _strategies.TryGetValue(configType, out var strategy) && strategy.Exists(configName);
        }

        /// <summary>
        /// 获取应用程序配置值。
        /// </summary>
        /// <param name="configName">配置键名。</param>
        /// <returns>配置值，如果不存在则返回 null。</returns>
        [Obsolete("Use IConfiguration[\"key\"] or IConfiguration.GetValue<string>(\"key\") instead.")]
        public static string GetAppConfig(string configName)
        {
            return _strategies[ConfigType.AppSettings].Get(configName);
        }

        /// <summary>
        /// 设置应用程序配置值。
        /// </summary>
        /// <param name="configName">配置键名。</param>
        /// <param name="configValue">配置值。</param>
        [Obsolete("Use IConnectionConfigProvider.SetConnectionString() for connection strings, or write to appsettings.json directly.")]
        public static void SetAppConfig(string configName, string configValue)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                throw new ArgumentNullException(nameof(configName));
            }
            _strategies[ConfigType.AppSettings].Set(configName, configValue, null);
        }

        /// <summary>
        /// 获取连接字符串。
        /// </summary>
        /// <param name="connectionName">连接名称。</param>
        /// <returns>连接字符串，如果不存在则返回 null。</returns>
        [Obsolete("Use IConfiguration.GetConnectionString() or IConnectionConfigProvider.GetConnectionString() instead.")]
        public static string GetConnectionString(string connectionName)
        {
            return _strategies[ConfigType.ConnectionString].Get(connectionName);
        }

        /// <summary>
        /// 设置连接字符串。
        /// </summary>
        /// <param name="connectionName">连接名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">提供者名称（可选）。</param>
        [Obsolete("Use IConnectionConfigProvider.SetConnectionString() instead.")]
        public static void SetConnectionString(string connectionName, string connectionString, string providerName = null)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                throw new ArgumentNullException(nameof(connectionName));
            }
            _strategies[ConfigType.ConnectionString].Set(connectionName, connectionString, providerName);
        }

        #region Private Strategies

        private abstract class ConfigStrategy
        {
            internal abstract bool Exists(string name);
            internal abstract string Get(string name);
            internal abstract void Set(string name, string value, string extra);

            protected System.Configuration.Configuration OpenConfig() =>
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
