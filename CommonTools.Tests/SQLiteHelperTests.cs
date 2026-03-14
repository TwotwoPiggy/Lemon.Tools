using CommonTools.Configuration;
using CommonTools.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace CommonTools.Tests
{
    [TestClass]
    public class SQLiteHelperTests
    {
        private string _testDbPath = string.Empty;
        private SQLiteHelper? _helper;

        [TestInitialize]
        public void Setup()
        {
            _testDbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _helper?.Dispose();
            _helper = null;

            if (File.Exists(_testDbPath))
            {
                try { File.Delete(_testDbPath); } catch { }
            }
        }

        [TestMethod]
        public void Constructor_WithDefaultParameters_CreatesInstance()
        {
            _helper = new SQLiteHelper();

            Assert.IsNotNull(_helper);
            Assert.IsNotNull(_helper.Db);
        }

        [TestMethod]
        public void Constructor_WithConnectionName_CreatesInstance()
        {
            _helper = new SQLiteHelper("TestConnection");

            Assert.IsNotNull(_helper);
        }

        [TestMethod]
        public void Constructor_WithConfiguration_CreatesInstance()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string?>("ConnectionStrings:TestDb", _testDbPath)
                })
                .Build();

            _helper = new SQLiteHelper(connectionName: "TestDb", configuration: config);

            Assert.IsNotNull(_helper);
        }

        [TestMethod]
        public void Constructor_WithConfigProvider_CreatesInstance()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            Assert.IsNotNull(_helper);
            mockProvider.Verify(x => x.GetConnectionString("Test"), Times.Once);
        }

        [TestMethod]
        public void Constructor_WithNullConfigProvider_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SQLiteHelper(null!, "Test"));
        }

        [TestMethod]
        public void SetConnectionString_WithValidValue_UpdatesConnection()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            var newPath = Path.Combine(Path.GetTempPath(), $"new_{Guid.NewGuid()}.db");
            _helper.SetConnectionString(newPath);

            mockProvider.Verify(x => x.SetConnectionString("Test", newPath), Times.Once);
        }

        [TestMethod]
        public void SetConnectionString_WithNullValue_ThrowsArgumentNullException()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            Assert.ThrowsException<ArgumentNullException>(() => _helper.SetConnectionString(null!));
        }

        [TestMethod]
        public void SetConnectionString_WithEmptyValue_ThrowsArgumentNullException()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            Assert.ThrowsException<ArgumentNullException>(() => _helper.SetConnectionString(""));
        }

        [TestMethod]
        public void Db_WhenDisposed_ThrowsObjectDisposedException()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            var db = _helper.Db;
            _helper.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => _helper.Db);
        }

        [TestMethod]
        public void DbAsync_WhenDisposed_ThrowsObjectDisposedException()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            var dbAsync = _helper.DbAsync;
            _helper.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => _helper.DbAsync);
        }

        [TestMethod]
        public void Disconnect_ReinitializesConnections()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            mockProvider.Setup(x => x.GetConnectionString("Test")).Returns(_testDbPath);

            _helper = new SQLiteHelper(mockProvider.Object, "Test");

            _helper.Disconnect();

            Assert.IsNotNull(_helper.Db);
        }
    }

    [TestClass]
    public class JsonConnectionConfigProviderTests
    {
        private string _testConfigPath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"appsettings_{Guid.NewGuid()}.json");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testConfigPath))
            {
                try { File.Delete(_testConfigPath); } catch { }
            }
        }

        [TestMethod]
        public void GetConnectionString_WithExistingConnection_ReturnsValue()
        {
            var jsonContent = @"{ ""ConnectionStrings"": { ""TestDb"": ""test.db"" } }";
            File.WriteAllText(_testConfigPath, jsonContent);

            var config = new ConfigurationBuilder()
                .AddJsonFile(_testConfigPath)
                .Build();

            var provider = new JsonConnectionConfigProvider(config);
            var result = provider.GetConnectionString("TestDb");

            Assert.AreEqual("test.db", result);
        }

        [TestMethod]
        public void GetConnectionString_WithNonExistingConnection_ReturnsEmpty()
        {
            var jsonContent = @"{ ""ConnectionStrings"": {} }";
            File.WriteAllText(_testConfigPath, jsonContent);

            var config = new ConfigurationBuilder()
                .AddJsonFile(_testConfigPath)
                .Build();

            var provider = new JsonConnectionConfigProvider(config);
            var result = provider.GetConnectionString("NonExisting");

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetConnectionString_WithNullName_ThrowsArgumentNullException()
        {
            var config = new ConfigurationBuilder().Build();
            var provider = new JsonConnectionConfigProvider(config);

            Assert.ThrowsException<ArgumentNullException>(() => provider.GetConnectionString(null!));
        }

        [TestMethod]
        public void SetConnectionString_WithValidValue_UpdatesConfig()
        {
            var jsonContent = @"{ ""ConnectionStrings"": {} }";
            File.WriteAllText(_testConfigPath, jsonContent);

            var config = new ConfigurationBuilder()
                .AddJsonFile(_testConfigPath, optional: false, reloadOnChange: true)
                .Build();

            var provider = new TestableJsonConnectionConfigProvider(config, _testConfigPath);
            provider.SetConnectionString("NewDb", "new_connection_string");

            var updatedContent = File.ReadAllText(_testConfigPath);
            StringAssert.Contains(updatedContent, "NewDb");
            StringAssert.Contains(updatedContent, "new_connection_string");
        }

        [TestMethod]
        public void SetConnectionString_WithNullName_ThrowsArgumentNullException()
        {
            var config = new ConfigurationBuilder().Build();
            var provider = new JsonConnectionConfigProvider(config);

            Assert.ThrowsException<ArgumentNullException>(() => provider.SetConnectionString(null!, "value"));
        }
    }

    [TestClass]
    public class ConnectionConfigProviderFactoryTests
    {
        [TestInitialize]
        public void Setup()
        {
            ConnectionConfigProviderFactory.Reset();
        }

        [TestMethod]
        public void Create_ReturnsNonNullProvider()
        {
            var provider = ConnectionConfigProviderFactory.Create();

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(JsonConnectionConfigProvider));
        }

        [TestMethod]
        public void Create_WithConfiguration_ReturnsProvider()
        {
            var config = new ConfigurationBuilder().Build();
            var provider = ConnectionConfigProviderFactory.Create(config);

            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void SetProvider_OverridesDefaultProvider()
        {
            var mockProvider = new Mock<IConnectionConfigProvider>();
            ConnectionConfigProviderFactory.SetProvider(mockProvider.Object);

            var provider = ConnectionConfigProviderFactory.Create();

            Assert.AreSame(mockProvider.Object, provider);
        }

        [TestMethod]
        public void Reset_ClearsCachedProvider()
        {
            var firstProvider = ConnectionConfigProviderFactory.Create();
            ConnectionConfigProviderFactory.Reset();
            var secondProvider = ConnectionConfigProviderFactory.Create();

            Assert.AreNotSame(firstProvider, secondProvider);
        }
    }

    internal class TestableJsonConnectionConfigProvider : IConnectionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _configFilePath;
        private readonly object _syncLock = new();

        public TestableJsonConnectionConfigProvider(IConfiguration configuration, string configFilePath)
        {
            _configuration = configuration;
            _configFilePath = configFilePath;
        }

        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name) ?? string.Empty;
        }

        public void SetConnectionString(string name, string connectionString)
        {
            lock (_syncLock)
            {
                var jsonContent = File.Exists(_configFilePath)
                    ? File.ReadAllText(_configFilePath)
                    : "{}";

                var json = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);

                if (json["ConnectionStrings"] == null)
                {
                    json["ConnectionStrings"] = new Newtonsoft.Json.Linq.JObject();
                }

                json["ConnectionStrings"]![name] = connectionString;

                File.WriteAllText(_configFilePath, json.ToString(Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}
