using Fanex.Athena.BetTicket.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Helpers
{
    public class ErrorMessageHelperTests
    {
        [Fact]
        public void GenerateMessage_WithSimpleException_ShouldReturnFormattedMessage()
        {
            // Arrange
            var exception = new InvalidOperationException("Test error message");

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception);

            // Assert
            Assert.Contains("ErrMsg: Test error message", result);
            Assert.Contains("EXCEPTION INFO", result);
            Assert.Contains("Message: Test error message", result);
            Assert.Contains("Source: ", result);
            Assert.Contains("TargetSite: ", result);
            Assert.Contains("StackTrace: ", result);
        }

        [Fact]
        public void GenerateMessage_WithNullContext_ShouldNotIncludeRequestInfo()
        {
            // Arrange
            var exception = new Exception("Test message");

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception, null);

            // Assert
            Assert.DoesNotContain("REQUEST INFO", result);
            Assert.Contains("DATABASE INFO", result);
            Assert.Contains("EXCEPTION INFO", result);
        }

        [Fact]
        public void GenerateMessage_WithHttpContext_ShouldIncludeRequestInfo()
        {
            // Arrange
            var exception = new Exception("Test message");
            var context = CreateMockHttpContext();

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception, context);

            // Assert
            Assert.Contains("REQUEST INFO", result);
            Assert.Contains("Host: ", result);
            Assert.Contains("Path: ", result);
            Assert.Contains("Remote IP: ", result);
            Assert.Contains("DATABASE INFO", result);
            Assert.Contains("EXCEPTION INFO", result);
        }

        [Fact]
        public void GenerateMessage_WithInnerExceptions_ShouldLimitInnerExceptionDepth()
        {
            // Arrange
            var innerException3 = new Exception("Level 3");
            var innerException2 = new Exception("Level 2", innerException3);
            var innerException1 = new Exception("Level 1", innerException2);
            var mainException = new Exception("Main exception", innerException1);

            // Act
            var result = ErrorMessageHelper.GenerateMessage(mainException);

            // Assert
            Assert.Contains("Message: Main exception", result);
            Assert.Contains("Message: Level 1", result);
            Assert.Contains("Message: Level 2", result);
            Assert.Contains("Message: Level 3", result);
        }

        [Fact]
        public void GenerateMessage_WithNullException_ShouldThrowNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => ErrorMessageHelper.GenerateMessage(null));
        }

        [Fact]
        public void GenerateMessage_WithDatabaseInfo_ShouldIncludeDatabaseSection()
        {
            // Arrange
            var exception = new Exception("Test message");

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception);

            // Assert
            Assert.Contains("DATABASE INFO", result);
        }

        [Fact]
        public void GenerateMessage_WithException_ShouldIncludeAllSections()
        {
            // Arrange
            var exception = new ArgumentException("Test argument exception");
            var context = CreateMockHttpContext();

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception, context);

            // Assert
            Assert.Contains("ErrMsg: Test argument exception", result);
            Assert.Contains("REQUEST INFO", result);
            Assert.Contains("DATABASE INFO", result);
            Assert.Contains("EXCEPTION INFO", result);
        }

        [Fact]
        public void GenerateMessage_WithStackTrace_ShouldIncludeStackTrace()
        {
            // Arrange
            Exception exception;
            try
            {
                throw new InvalidOperationException("Test exception with stack trace");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Act
            var result = ErrorMessageHelper.GenerateMessage(exception);

            // Assert
            Assert.Contains("StackTrace: ", result);
            Assert.Contains("GenerateMessage_WithStackTrace_ShouldIncludeStackTrace", result);
        }

        private static HttpContext CreateMockHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost", 5000);
            context.Request.Path = "/api/test";
            context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            return context;
        }
    }
} 