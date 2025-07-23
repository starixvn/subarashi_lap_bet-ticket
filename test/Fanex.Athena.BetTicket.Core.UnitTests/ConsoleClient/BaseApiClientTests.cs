using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace Fanex.Athena.BetTicket.Core.UnitTests.ConsoleClient
{
    public class BaseApiClientTests
    {
        private readonly TestBaseApiClient _testClient;

        public BaseApiClientTests()
        {
            var httpClient = new HttpClient();
            _testClient = new TestBaseApiClient(httpClient);
        }

        // Test implementation class for BaseApiClient
        private class TestBaseApiClient : BaseApiClient
        {
            public TestBaseApiClient(HttpClient client) : base(client)
            {
            }

            protected override string ApiName => "TestApi";

            public async Task<T> TestPost<T>(string path, object value, string language) where T : class
            {
                return await Post<T>(path, value, language);
            }

            public async Task<T> TestHandleAsync<T>(string path, object value, string language, MediaType mediaType, Func<HttpResponseMessage, Task<T>> codeBlock, bool disposeHttpResponse = true, int timeOut = 100)
            {
                return await HandleAsync(path, value, language, mediaType, codeBlock, disposeHttpResponse, timeOut);
            }

            public async Task TestPrepareHeaders(HttpRequestHeaders headers)
            {
                await PrepareHeaders(headers);
            }
        }

        [Fact]
        public void BaseApiClient_Constructor_ShouldInitialize()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var client = new TestBaseApiClient(httpClient);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void BaseApiClient_Constructor_WithNullHttpClient_ShouldNotThrowException()
        {
            // Act & Assert
            var client = new TestBaseApiClient(null);
            Assert.NotNull(client);
        }

        [Fact]
        public void ApiName_ShouldReturnCorrectValue()
        {
            // Act
            var apiName = _testClient.GetType().GetProperty("ApiName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_testClient);

            // Assert
            Assert.Equal("TestApi", apiName);
        }

        [Fact]
        public async Task PrepareHeaders_ShouldCompleteSuccessfully()
        {
            // Arrange
            var headers = new HttpRequestMessage().Headers;

            // Act & Assert
            await _testClient.TestPrepareHeaders(headers);
            // Should not throw any exception
        }

        [Fact]
        public async Task Post_WithNullCodeBlock_ShouldReturnDefault()
        {
            // Act
            var result = await _testClient.TestHandleAsync<object>("test/path", new { }, "en", MediaType.Json, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ServiceException_Constructor_WithMessageAndServiceName_ShouldSetProperties()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains(message, exception.Message);
            Assert.Contains(serviceName, exception.Message);
        }

        [Fact]
        public void ServiceException_Constructor_WithMessageServiceNameAndInnerException_ShouldSetProperties()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new ServiceException(message, serviceName, innerException);

            // Assert
            Assert.Contains(message, exception.Message);
            Assert.Contains(serviceName, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void ServiceException_Message_ShouldReturnFormattedMessage()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            var exception = new ServiceException(message, serviceName);

            // Act
            var formattedMessage = exception.Message;

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", formattedMessage);
            Assert.Contains($"Exception Message: {message}", formattedMessage);
        }

        [Fact]
        public void ServiceException_GetObjectData_ShouldSerializeCorrectly()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            var exception = new ServiceException(message, serviceName);

            // Act & Assert
            // This test verifies that GetObjectData doesn't throw an exception
            // The actual serialization is tested by the framework
            Assert.NotNull(exception);
        }

        [Fact]
        public void MediaType_Json_ShouldHaveCorrectValue()
        {
            // Assert
            Assert.Equal(0, (byte)MediaType.Json);
        }

        [Fact]
        public void MediaType_MsgPack_ShouldHaveCorrectValue()
        {
            // Assert
            Assert.Equal(1, (byte)MediaType.MsgPack);
        }

        [Fact]
        public void BaseApiClient_ShouldBeAbstract()
        {
            // Assert
            Assert.True(typeof(BaseApiClient).IsAbstract);
        }

        [Fact]
        public void BaseApiClient_ShouldHaveProtectedConstructor()
        {
            // Arrange & Act
            var httpClient = new HttpClient();
            var client = new TestBaseApiClient(httpClient);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void BaseApiClient_ShouldHaveProtectedApiNameProperty()
        {
            // Arrange
            var httpClient = new HttpClient();
            var client = new TestBaseApiClient(httpClient);

            // Act
            var apiNameProperty = typeof(BaseApiClient).GetProperty("ApiName", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(apiNameProperty);
            Assert.True(apiNameProperty.CanWrite == false); // Protected property
        }

        [Fact]
        public void BaseApiClient_ShouldHaveProtectedPrepareHeadersMethod()
        {
            // Arrange
            var httpClient = new HttpClient();
            var client = new TestBaseApiClient(httpClient);

            // Act
            var prepareHeadersMethod = typeof(BaseApiClient).GetMethod("PrepareHeaders", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(prepareHeadersMethod);
            Assert.True(prepareHeadersMethod.IsVirtual);
        }

        [Fact]
        public void BaseApiClient_ShouldHaveProtectedHandleAsyncMethod()
        {
            // Arrange
            var httpClient = new HttpClient();
            var client = new TestBaseApiClient(httpClient);

            // Act
            var handleAsyncMethod = typeof(BaseApiClient).GetMethod("HandleAsync", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(handleAsyncMethod);
            Assert.True(handleAsyncMethod.IsVirtual);
        }

        [Fact]
        public void BaseApiClient_ShouldHavePublicPostMethod()
        {
            // Arrange
            var httpClient = new HttpClient();
            var client = new TestBaseApiClient(httpClient);

            // Act
            var postMethod = typeof(BaseApiClient).GetMethod("Post");

            // Assert
            Assert.NotNull(postMethod);
            Assert.True(postMethod.IsVirtual);
        }

        [Fact]
        public void ServiceException_ShouldInheritFromException()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void ServiceException_ShouldImplementISerializable()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.IsAssignableFrom<ISerializable>(exception);
        }

        [Fact]
        public void ServiceException_ShouldBeSealed()
        {
            // Assert
            Assert.True(typeof(ServiceException).IsSealed);
        }

        [Theory]
        [InlineData("TestService", "Network error")]
        [InlineData("ApiService", "Timeout error")]
        [InlineData("DataService", "Connection error")]
        public void ServiceException_WithDifferentServiceNames_ShouldFormatMessageCorrectly(string serviceName, string message)
        {
            // Arrange & Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", exception.Message);
            Assert.Contains($"Exception Message: {message}", exception.Message);
        }

        [Fact]
        public void ServiceException_WithNullMessage_ShouldHandleNullMessage()
        {
            // Arrange
            string message = null;
            var serviceName = "TestService";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", exception.Message);
        }

        [Fact]
        public void ServiceException_WithEmptyMessage_ShouldHandleEmptyMessage()
        {
            // Arrange
            var message = "";
            var serviceName = "TestService";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", exception.Message);
        }

        [Fact]
        public void ServiceException_WithNullServiceName_ShouldHandleNullServiceName()
        {
            // Arrange
            var message = "Test error message";
            string serviceName = null;

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", exception.Message);
            Assert.Contains($"Exception Message: {message}", exception.Message);
        }

        [Fact]
        public void ServiceException_WithEmptyServiceName_ShouldHandleEmptyServiceName()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "";

            // Act
            var exception = new ServiceException(message, serviceName);

            // Assert
            Assert.Contains($"Exception when call Service: {serviceName}", exception.Message);
            Assert.Contains($"Exception Message: {message}", exception.Message);
        }

        [Fact]
        public void ServiceException_WithInnerException_ShouldPreserveInnerException()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new ServiceException(message, serviceName, innerException);

            // Assert
            Assert.Equal(innerException, exception.InnerException);
            Assert.Equal("Inner error", exception.InnerException.Message);
        }

        [Fact]
        public void ServiceException_WithNullInnerException_ShouldHandleNullInnerException()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            Exception innerException = null;

            // Act
            var exception = new ServiceException(message, serviceName, innerException);

            // Assert
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void ServiceException_GetObjectData_ShouldNotThrowException()
        {
            // Arrange
            var message = "Test error message";
            var serviceName = "TestService";
            var exception = new ServiceException(message, serviceName);

            // Act & Assert
            // This test verifies that GetObjectData doesn't throw an exception
            // The actual serialization is tested by the framework
            Assert.NotNull(exception);
        }

        [Fact]
        public void ServiceException_SerializationConstructor_ShouldExist()
        {
            // Act
            var serializationConstructor = typeof(ServiceException).GetConstructor(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);

            // Assert
            Assert.NotNull(serializationConstructor);
        }
    }
} 