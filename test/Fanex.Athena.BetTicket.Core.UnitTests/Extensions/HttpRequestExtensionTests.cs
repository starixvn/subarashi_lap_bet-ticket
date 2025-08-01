using Fanex.Athena.BetTicket.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Extensions
{
    public class HttpRequestExtensionTests
    {
        [Fact]
        public void IsAjaxRequest_WithValidAjaxHeader_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Headers["X-Requested-With"] = "XMLHttpRequest";

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_WithValidAjaxHeaderCaseInsensitive_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Headers["X-Requested-With"] = "xmlhttprequest";

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_WithInvalidAjaxHeader_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Headers["X-Requested-With"] = "InvalidValue";

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_WithMissingAjaxHeader_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_WithEmptyAjaxHeader_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Headers["X-Requested-With"] = "";

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_WithMultipleAjaxHeaders_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Headers["X-Requested-With"] = new StringValues(new[] { "XMLHttpRequest", "OtherValue" });

            // Act
            var result = request.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStaticRequest_WithPathContainingDot_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = "/static/image.jpg";

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStaticRequest_WithPathContainingMultipleDots_ShouldReturnTrue()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = "/static/file.min.css";

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStaticRequest_WithPathNotContainingDot_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = "/api/users";

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStaticRequest_WithNullPath_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = null;

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStaticRequest_WithEmptyPath_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = "";

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStaticRequest_WithRootPath_ShouldReturnFalse()
        {
            // Arrange
            var request = CreateMockHttpRequest();
            request.Path = "/";

            // Act
            var result = request.IsStaticRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleInvalidRequest_WithAjaxRequest_ShouldSetJsonResult()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: true);
            var errorMsg = "Test error message";

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var jsonResult = Assert.IsType<JsonResult>(actionContext.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, jsonResult.StatusCode);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public void HandleInvalidRequest_WithNonAjaxRequest_ShouldSetRedirectResult()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: false);
            var errorMsg = "Test error message";

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var redirectResult = Assert.IsType<RedirectResult>(actionContext.Result);
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
            
            // Verify error message header was added
            Assert.True(actionContext.HttpContext.Response.Headers.ContainsKey("error-msg"));
            Assert.Equal(errorMsg, actionContext.HttpContext.Response.Headers["error-msg"]);
        }

        [Fact]
        public void HandleInvalidRequest_WithEmptyErrorMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: true);
            var errorMsg = "";

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var jsonResult = Assert.IsType<JsonResult>(actionContext.Result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public void HandleInvalidRequest_WithNullErrorMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: true);
            string errorMsg = null;

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var jsonResult = Assert.IsType<JsonResult>(actionContext.Result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public void HandleInvalidRequest_WithLongErrorMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: true);
            var errorMsg = new string('x', 1000); // 1KB error message

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var jsonResult = Assert.IsType<JsonResult>(actionContext.Result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public void HandleInvalidRequest_WithSpecialCharactersInErrorMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: true);
            var errorMsg = "Error with special chars: <>&\"'";

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var jsonResult = Assert.IsType<JsonResult>(actionContext.Result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public void HandleInvalidRequest_WithNonAjaxRequestAndSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var actionContext = CreateActionExecutingContext(isAjaxRequest: false);
            var errorMsg = "Error with special chars: <>&\"'";

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.NotNull(actionContext.Result);
            var redirectResult = Assert.IsType<RedirectResult>(actionContext.Result);
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
            Assert.Equal(errorMsg, actionContext.HttpContext.Response.Headers["error-msg"]);
        }

        #region Helper Methods

        private HttpRequest CreateMockHttpRequest()
        {
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            
            mockRequest.Setup(r => r.Headers).Returns(headers);
            mockRequest.SetupProperty(r => r.Path, new PathString("/test"));
            
            return mockRequest.Object;
        }

        private ActionExecutingContext CreateActionExecutingContext(bool isAjaxRequest = false)
        {
            var mockController = new Mock<Controller>();
            var mockActionDescriptor = new Mock<ActionDescriptor>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockResponse = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();
            var responseHeaders = new HeaderDictionary();

            // Setup request
            mockRequest.Setup(r => r.Headers).Returns(headers);
            mockRequest.Setup(r => r.Path).Returns(new PathString("/test"));
            
            // Setup AJAX header if needed
            if (isAjaxRequest)
            {
                headers["X-Requested-With"] = "XMLHttpRequest";
            }
            
            // Setup response
            mockResponse.Setup(r => r.Headers).Returns(responseHeaders);
            
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);

            return new ActionExecutingContext(
                new ActionContext(mockHttpContext.Object, new RouteData(), mockActionDescriptor.Object),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                mockController.Object
            );
        }

        #endregion
    }
} 