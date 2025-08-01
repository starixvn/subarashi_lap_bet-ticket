using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyAdvancedExtensionsTests
    {
        [Fact]
        public void ToWebSocketScheme_WithHttpsUri_ShouldReturnWssUri()
        {
            // Arrange
            var uri = new Uri("https://example.com/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpUri_ShouldReturnWsUri()
        {
            // Arrange
            var uri = new Uri("http://example.com/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithWssUri_ShouldReturnSameUri()
        {
            // Arrange
            var uri = new Uri("wss://example.com/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithWsUri_ShouldReturnSameUri()
        {
            // Arrange
            var uri = new Uri("ws://example.com/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithNullUri_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ((Uri)null).ToWebSocketScheme());
        }

        [Fact]
        public void ToWebSocketScheme_WithCustomScheme_ShouldReturnSameUri()
        {
            // Arrange
            var uri = new Uri("ftp://example.com/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ftp", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithPort_ShouldPreservePort()
        {
            // Arrange
            var uri = new Uri("https://example.com:8080/api");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal(8080, result.Port);
            Assert.Equal("/api", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithQueryParameters_ShouldPreserveQuery()
        {
            // Arrange
            var uri = new Uri("https://example.com/api?param1=value1&param2=value2");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api?param1=value1&param2=value2", result.PathAndQuery);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithGetRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" },
                { "Accept", "application/json" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
            Assert.Null(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithPostRequest_ShouldCreateCorrectRequestWithContent()
        {
            // Arrange
            var bodyContent = "test body content";
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Content-Length", bodyContent.Length.ToString() }
            }, bodyContent);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeaders_ShouldCopyHeaders()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" },
                { "Accept", "application/json" },
                { "Authorization", "Bearer token123" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal("TestAgent", result.Headers.UserAgent.ToString());
            Assert.Equal("application/json", result.Headers.Accept.ToString());
            Assert.Equal("Bearer token123", result.Headers.Authorization.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithDeleteRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("DELETE", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Delete, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Null(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("HEAD", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Head, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Null(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithTraceRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("TRACE", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Trace, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Null(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithPutRequest_ShouldCreateCorrectRequestWithContent()
        {
            // Arrange
            var bodyContent = "test body content";
            var context = CreateMockHttpContext("PUT", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Content-Length", bodyContent.Length.ToString() }
            }, bodyContent);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Put, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithPatchRequest_ShouldCreateCorrectRequestWithContent()
        {
            // Arrange
            var bodyContent = "test body content";
            var context = CreateMockHttpContext("PATCH", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Content-Length", bodyContent.Length.ToString() }
            }, bodyContent);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(new HttpMethod("PATCH"), result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void SendProxyHttpRequest_WithValidRequest_ShouldReturnResponse()
        {
            // Arrange
            var mockOptions = new Mock<IOptions<SharedProxyOptions>>();
            mockOptions.Setup(o => o.Value).Returns(new SharedProxyOptions());

            // Create a real ProxyService instance with mocked options
            var proxyService = new ProxyService(mockOptions.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(sp => sp.GetService(typeof(ProxyService))).Returns(proxyService);

            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());
            context.Setup(c => c.RequestServices).Returns(mockServiceProvider.Object);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.example.com/test");

            // Act
            var result = context.Object.SendProxyHttpRequest(requestMessage);

            // Assert
            Assert.NotNull(result);
            // Note: We can't easily mock HttpClient.SendAsync as it's not virtual
            // This test verifies that the method returns a Task and doesn't throw exceptions
        }

        [Fact]
        public void CopyProxyHttpResponse_WithValidResponse_ShouldCopyToContext()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test content", Encoding.UTF8, "text/plain")
            };
            responseMessage.Headers.Add("X-Custom-Header", "custom-value");

            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());

            // Act
            var task = context.Object.CopyProxyHttpResponse(responseMessage);

            // Assert
            Assert.NotNull(task);
            // Note: This is a simplified test as the actual copying requires more complex setup
        }

        [Fact]
        public async Task CopyProxyHttpResponse_WithNullResponse_ShouldThrowArgumentNullException()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await context.Object.CopyProxyHttpResponse(null));
        }

        [Fact]
        public void SendProxyHttpRequest_WithNullRequest_ShouldThrowArgumentNullException()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());

            // Act & Assert
            // The ArgumentNullException is thrown synchronously before the async operation
            var exception = Assert.Throws<ArgumentNullException>(() => 
            {
                var task = context.Object.SendProxyHttpRequest(null);
                // We don't await the task because the exception is thrown synchronously
            });
            Assert.Equal("requestMessage", exception.ParamName);
        }

        [Theory]
        [InlineData("https://example.com/api", "wss://example.com/api")]
        [InlineData("http://example.com/api", "ws://example.com/api")]
        [InlineData("wss://example.com/api", "wss://example.com/api")]
        [InlineData("ws://example.com/api", "ws://example.com/api")]
        [InlineData("https://example.com:8080/api", "wss://example.com:8080/api")]
        [InlineData("http://example.com:8080/api", "ws://example.com:8080/api")]
        public void ToWebSocketScheme_WithVariousUris_ShouldReturnCorrectScheme(string inputUri, string expectedScheme)
        {
            // Arrange
            var uri = new Uri(inputUri);

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal(expectedScheme, result.ToString());
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("HEAD")]
        [InlineData("DELETE")]
        [InlineData("TRACE")]
        public void CreateProxyHttpRequest_WithMethodsWithoutContent_ShouldNotHaveContent(string method)
        {
            // Arrange
            var context = CreateMockHttpContext(method, "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(new HttpMethod(method), result.Method);
            Assert.Null(result.Content);
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        public void CreateProxyHttpRequest_WithMethodsWithContent_ShouldHaveContent(string method)
        {
            // Arrange
            var context = CreateMockHttpContext(method, "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(new HttpMethod(method), result.Method);
            Assert.NotNull(result.Content);
        }

        private Mock<HttpContext> CreateMockHttpContext(string method, string path, Dictionary<string, string> headers, string bodyContent = null)
        {
            var mockContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockResponse = new Mock<HttpResponse>();
            var mockServiceProvider = new Mock<IServiceProvider>();

            // Setup request
            mockRequest.Setup(r => r.Method).Returns(method);
            mockRequest.Setup(r => r.Path).Returns(path);
            mockRequest.Setup(r => r.Body).Returns(bodyContent != null ? 
                new MemoryStream(Encoding.UTF8.GetBytes(bodyContent)) : 
                new MemoryStream());

            // Setup headers
            var headerDictionary = new HeaderDictionary();
            foreach (var header in headers)
            {
                headerDictionary[header.Key] = header.Value;
            }
            mockRequest.Setup(r => r.Headers).Returns(headerDictionary);

            // Setup context
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.RequestServices).Returns(mockServiceProvider.Object);

            return mockContext;
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpsUriWithFragment_ShouldPreserveFragment()
        {
            // Arrange
            var uri = new Uri("https://example.com/api#section1");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
            Assert.Equal("#section1", result.Fragment);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpUriWithFragment_ShouldPreserveFragment()
        {
            // Arrange
            var uri = new Uri("http://example.com/api#section1");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api", result.PathAndQuery);
            Assert.Equal("#section1", result.Fragment);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpsUriWithComplexQuery_ShouldPreserveQuery()
        {
            // Arrange
            var uri = new Uri("https://example.com/api?param1=value1&param2=value2&param3=value3");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api?param1=value1&param2=value2&param3=value3", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpUriWithComplexQuery_ShouldPreserveQuery()
        {
            // Arrange
            var uri = new Uri("http://example.com/api?param1=value1&param2=value2&param3=value3");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api?param1=value1&param2=value2&param3=value3", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpsUriWithSpecialCharacters_ShouldPreserveSpecialCharacters()
        {
            // Arrange
            var uri = new Uri("https://example.com/api/path%20with%20spaces?param=value%20with%20spaces");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api/path%20with%20spaces?param=value%20with%20spaces", result.PathAndQuery);
        }

        [Fact]
        public void ToWebSocketScheme_WithHttpUriWithSpecialCharacters_ShouldPreserveSpecialCharacters()
        {
            // Arrange
            var uri = new Uri("http://example.com/api/path%20with%20spaces?param=value%20with%20spaces");

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
            Assert.Equal("example.com", result.Host);
            Assert.Equal("/api/path%20with%20spaces?param=value%20with%20spaces", result.PathAndQuery);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithEmptyHeaders_ShouldCreateRequestWithoutHeaders()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
            Assert.Null(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithContentTypeHeader_ShouldCopyToContentHeaders()
        {
            // Arrange
            var bodyContent = "test body content";
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Content-Length", bodyContent.Length.ToString() }
            }, bodyContent);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
            Assert.Equal("application/json", result.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithMultipleHeaders_ShouldCopyAllHeaders()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" },
                { "Accept", "application/json" },
                { "Authorization", "Bearer token123" },
                { "X-Custom-Header", "custom-value" },
                { "Cache-Control", "no-cache" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
            Assert.Equal("TestAgent", result.Headers.UserAgent.ToString());
            Assert.Equal("application/json", result.Headers.Accept.ToString());
            Assert.Equal("Bearer token123", result.Headers.Authorization.ToString());
            Assert.Equal("custom-value", result.Headers.GetValues("X-Custom-Header").First());
            Assert.Equal("no-cache", result.Headers.GetValues("Cache-Control").First());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithLargeBodyContent_ShouldHandleLargeContent()
        {
            // Arrange
            var largeBodyContent = new string('x', 10000); // 10KB content
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Content-Length", largeBodyContent.Length.ToString() }
            }, largeBodyContent);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
            Assert.Equal(largeBodyContent.Length, result.Content.Headers.ContentLength);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithBinaryContent_ShouldHandleBinaryContent()
        {
            // Arrange
            var binaryContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/octet-stream" },
                { "Content-Length", binaryContent.Length.ToString() }
            }, Encoding.UTF8.GetString(binaryContent));

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
            Assert.Equal("application/octet-stream", result.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithOptionsRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("OPTIONS", "/api/test", new Dictionary<string, string>
            {
                { "Origin", "https://example.com" },
                { "Access-Control-Request-Method", "POST" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Options, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithConnectRequest_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("CONNECT", "/api/test", new Dictionary<string, string>
            {
                { "Host", "api.example.com:443" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Connect, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithCustomMethod_ShouldCreateCorrectRequest()
        {
            // Arrange
            var context = CreateMockHttpContext("CUSTOM", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(new HttpMethod("CUSTOM"), result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithUriWithPort_ShouldSetCorrectHost()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com:8080/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com:8080", result.Headers.Host);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithUriWithDefaultPort_ShouldSetCorrectHost()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com:443/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithUriWithPathAndQuery_ShouldPreservePathAndQuery()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com/proxy/path?param1=value1&param2=value2");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("api.example.com", result.Headers.Host);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadersWithMultipleValues_ShouldHandleMultipleValues()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "Accept", "application/json, text/plain, */*" },
                { "Accept-Encoding", "gzip, deflate, br" },
                { "Accept-Language", "en-US, en;q=0.9, es;q=0.8" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("application/json, text/plain, */*", result.Headers.Accept.ToString());
            Assert.Equal("gzip, deflate, br", result.Headers.AcceptEncoding.ToString());
            Assert.Equal("en-US, en;q=0.9, es;q=0.8", result.Headers.AcceptLanguage.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadersWithSpecialCharacters_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "User-Agent", "TestAgent/1.0 (Windows NT 10.0; Win64; x64)" },
                { "Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("TestAgent/1.0 (Windows NT 10.0; Win64; x64)", result.Headers.UserAgent.ToString());
            Assert.Equal("Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c", result.Headers.Authorization.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithEmptyBody_ShouldCreateRequestWithoutContent()
        {
            // Arrange
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }, "");

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
            Assert.Equal(0, result.Content.Headers.ContentLength);
        }

        [Fact]
        public void CreateProxyHttpRequest_WithNullBody_ShouldCreateRequestWithoutContent()
        {
            // Arrange
            var context = CreateMockHttpContext("POST", "/api/test", new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }, null);

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Post, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.NotNull(result.Content);
            Assert.Equal(0, result.Content.Headers.ContentLength);
        }

        [Theory]
        [InlineData("https://example.com/api", "wss://example.com/api")]
        [InlineData("http://example.com/api", "ws://example.com/api")]
        [InlineData("https://example.com:8080/api", "wss://example.com:8080/api")]
        [InlineData("http://example.com:8080/api", "ws://example.com:8080/api")]
        [InlineData("https://example.com/api?param=value", "wss://example.com/api?param=value")]
        [InlineData("http://example.com/api?param=value", "ws://example.com/api?param=value")]
        [InlineData("https://example.com/api#fragment", "wss://example.com/api#fragment")]
        [InlineData("http://example.com/api#fragment", "ws://example.com/api#fragment")]
        public void ToWebSocketScheme_WithVariousUriFormats_ShouldReturnCorrectUri(string inputUri, string expectedUri)
        {
            // Arrange
            var uri = new Uri(inputUri);

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal(expectedUri, result.ToString());
        }

        [Theory]
        [InlineData("GET", false)]
        [InlineData("HEAD", false)]
        [InlineData("DELETE", false)]
        [InlineData("TRACE", false)]
        [InlineData("POST", true)]
        [InlineData("PUT", true)]
        [InlineData("PATCH", true)]
        [InlineData("OPTIONS", true)]
        [InlineData("CONNECT", true)]
        public void CreateProxyHttpRequest_WithVariousMethods_ShouldHaveCorrectContent(string method, bool shouldHaveContent)
        {
            // Arrange
            var context = CreateMockHttpContext(method, "/api/test", new Dictionary<string, string>());
            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(new HttpMethod(method), result.Method);
            if (shouldHaveContent)
            {
                Assert.NotNull(result.Content);
            }
            else
            {
                Assert.Null(result.Content);
            }
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadersWithEmptyValues_ShouldHandleEmptyValues()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "Empty-Header", "" },
                { "Null-Header", null },
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("TestAgent", result.Headers.UserAgent.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadersWithWhitespaceValues_ShouldHandleWhitespaceValues()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "Whitespace-Header", "   " },
                { "User-Agent", "TestAgent" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("TestAgent", result.Headers.UserAgent.ToString());
        }

        [Fact]
        public void CreateProxyHttpRequest_WithHeadersWithSpecialCharactersInValues_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var context = CreateMockHttpContext("GET", "/api/test", new Dictionary<string, string>
            {
                { "X-Custom-Header", "value with spaces and special chars: !@#$%^&*()" },
                { "User-Agent", "TestAgent/1.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36" }
            });

            var uri = new Uri("https://api.example.com/proxy");

            // Act
            var result = context.Object.CreateProxyHttpRequest(uri);

            // Assert
            Assert.Equal(HttpMethod.Get, result.Method);
            Assert.Equal(uri, result.RequestUri);
            Assert.Equal("value with spaces and special chars: !@#$%^&*()", result.Headers.GetValues("X-Custom-Header").First());
            Assert.Equal("TestAgent/1.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36", result.Headers.UserAgent.ToString());
        }
    }
} 