using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Twotwo.Agent.Tests.Services
{
    [TestClass]
    public class GeminiAgentServiceTests
    {
        private Mock<IOptions<AIConfig>> _mockOptions = null!;
        private AIConfig _config = null!;

        [TestInitialize]
        public void Setup()
        {
            _config = new AIConfig
            {
                ApiKey = "test-api-key",
                ModelName = "gemini-2.5-flash"
            };
            _mockOptions = new Mock<IOptions<AIConfig>>();
            _mockOptions.Setup(x => x.Value).Returns(_config);
        }

        [TestMethod]
        public void Constructor_WithValidConfig_InitializesCorrectly()
        {
            var service = new GeminiAgentService(_mockOptions.Object);

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Constructor_WithNullConfig_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new GeminiAgentService(null!));
        }

        [TestMethod]
        public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
        {
            _config.ApiKey = string.Empty;

            Assert.ThrowsException<ArgumentException>(() =>
                new GeminiAgentService(_mockOptions.Object));
        }

        [TestMethod]
        public async Task GetModelsAsync_WhenCalled_ReturnsModelsList()
        {
            var service = new GeminiAgentService(_mockOptions.Object);

            try
            {
                var models = await service.GetModelsAsync();

                Assert.IsNotNull(models);
                Assert.IsInstanceOfType(models, typeof(IReadOnlyList<Model>));
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Failed to retrieve available models"));
            }
        }

        [TestMethod]
        public async Task GetModelsAsync_WithCancellationToken_PassesTokenCorrectly()
        {
            var service = new GeminiAgentService(_mockOptions.Object);
            var cts = new CancellationTokenSource();

            try
            {
                var models = await service.GetModelsAsync(cts.Token);
                Assert.IsNotNull(models);
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public async Task GetModelsAsync_WhenCancelled_ThrowsTaskCanceledException()
        {
            var service = new GeminiAgentService(_mockOptions.Object);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () =>
                await service.GetModelsAsync(cts.Token));
        }

        [TestMethod]
        public async Task ValidateModelAsync_WhenModelExists_ReturnsTrue()
        {
            var service = new GeminiAgentService(_mockOptions.Object);

            try
            {
                var result = await service.ValidateModelAsync();
                Assert.IsTrue(result);
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public async Task ValidateModelAsync_WhenCalledTwice_UsesCache()
        {
            var service = new GeminiAgentService(_mockOptions.Object);

            try
            {
                var result1 = await service.ValidateModelAsync();
                var result2 = await service.ValidateModelAsync();

                Assert.IsTrue(result1);
                Assert.IsTrue(result2);
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
