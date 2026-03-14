using CommonTools.Configuration;
using CommonTools.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommonTools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSQLiteHelper(
            this IServiceCollection services,
            string connectionName = "Default")
        {
            services.AddSingleton<IConnectionConfigProvider>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return ConnectionConfigProviderFactory.Create(configuration);
            });

            services.AddSingleton<SQLiteHelper>(sp =>
            {
                var configProvider = sp.GetRequiredService<IConnectionConfigProvider>();
                return new SQLiteHelper(configProvider, connectionName);
            });

            return services;
        }

        public static IServiceCollection AddSQLiteHelper(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionName = "Default")
        {
            services.AddSingleton<IConnectionConfigProvider>(sp =>
                ConnectionConfigProviderFactory.Create(configuration));

            services.AddSingleton<SQLiteHelper>(sp =>
            {
                var configProvider = sp.GetRequiredService<IConnectionConfigProvider>();
                return new SQLiteHelper(configProvider, connectionName);
            });

            return services;
        }

        public static IServiceCollection AddSQLiteHelper(
            this IServiceCollection services,
            Action<SQLiteConnectionOptions> configure)
        {
            services.Configure(configure);

            services.AddSingleton<IConnectionConfigProvider>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return ConnectionConfigProviderFactory.Create(configuration);
            });

            services.AddSingleton<SQLiteHelper>(sp =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<SQLiteConnectionOptions>>();
                var configProvider = sp.GetRequiredService<IConnectionConfigProvider>();
                
                var connectionName = options.Value.DefaultConnectionName;
                var connectionString = options.Value.ConnectionString;

                if (!string.IsNullOrEmpty(connectionString))
                {
                    configProvider.SetConnectionString(connectionName, connectionString);
                }

                return new SQLiteHelper(configProvider, connectionName);
            });

            return services;
        }
    }
}
