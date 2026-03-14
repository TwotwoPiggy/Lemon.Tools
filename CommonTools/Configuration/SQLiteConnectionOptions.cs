namespace CommonTools.Configuration
{
    public class SQLiteConnectionOptions
    {
        public const string SectionName = "SQLite";

        public string ConnectionString { get; set; } = string.Empty;
        public string DefaultConnectionName { get; set; } = "Default";
    }

    public class ConnectionStringsOptions
    {
        public const string SectionName = "ConnectionStrings";

        public string? SQLite { get; set; }
        public string? MSSQL { get; set; }
        public string? Default { get; set; }
    }
}
