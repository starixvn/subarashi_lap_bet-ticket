using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Attributes
{
    public class DataValidationAttributeTests
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILicenseeService> _mockLicenseeService;

        public DataValidationAttributeTests()
        {
            _mockContextService = new Mock<IContextService>();
            _mockLicenseeService = new Mock<ILicenseeService>();
        }

        #region DataValidationAttribute Tests

        [Fact]
        public void Constructor_WithDefaultParameters_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new DataValidationAttribute();

            // Assert
            Assert.Equal(typeof(DataValidationImplAttribute), attribute.ImplementationType);
            Assert.Single(attribute.Arguments);
            Assert.False((bool)attribute.Arguments[0]); // validateCustId
        }

        [Fact]
        public void Constructor_WithValidateCustIdTrue_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new DataValidationAttribute(validateCustId: true);

            // Assert
            Assert.Equal(typeof(DataValidationImplAttribute), attribute.ImplementationType);
            Assert.Single(attribute.Arguments);
            Assert.True((bool)attribute.Arguments[0]); // validateCustId
        }

        [Fact]
        public void Constructor_WithValidateCustIdFalse_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new DataValidationAttribute(validateCustId: false);

            // Assert
            Assert.Equal(typeof(DataValidationImplAttribute), attribute.ImplementationType);
            Assert.Single(attribute.Arguments);
            Assert.False((bool)attribute.Arguments[0]); // validateCustId
        }

        #endregion

        #region DataValidationImplAttribute Tests

        [Fact]
        public void Constructor_ShouldSetAllProperties()
        {
            // Arrange
            var validateCustId = true;

            // Act
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId
            );

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidationPasses_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: false
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments);
            var nextCalled = false;
            var next = new ActionExecutionDelegate(async () => 
            {
                nextCalled = true;
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidationFails_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: false
            );

            var invalidMetaModel = new TestInvalidValidationMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", invalidMetaModel } };
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsTrueAndCustomerBelongsTo_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var customerIdentityMetaModel = new TestCustomerIdentityMetaModel { CustId = 123, TransId = 456 };
            var actionArguments = new Dictionary<string, object> { { "model", customerIdentityMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var nextCalled = false;
            var next = new ActionExecutionDelegate(async () => 
            {
                nextCalled = true;
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongsTo(agentList, 123, 456))
                .ReturnsAsync(true);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
            _mockLicenseeService.Verify(x => x.CheckCustomerBelongsTo(agentList, 123, 456), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsTrueAndCustomerDoesNotBelong_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var customerIdentityMetaModel = new TestCustomerIdentityMetaModel { CustId = 123, TransId = 456 };
            var actionArguments = new Dictionary<string, object> { { "model", customerIdentityMetaModel } };
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongsTo(agentList, 123, 456))
                .ReturnsAsync(false);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
            _mockLicenseeService.Verify(x => x.CheckCustomerBelongsTo(agentList, 123, 456), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsTrueButNoCustomerIdentityMetaModel_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsFalseAndInvalidMetaModel_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: false
            );

            var invalidMetaModel = new TestInvalidValidationMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", invalidMetaModel } };
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsFalseAndValidMetaModel_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: false
            );

            var validMetaModel = new TestValidValidationMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", validMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var nextCalled = false;
            var next = new ActionExecutionDelegate(async () => 
            {
                nextCalled = true;
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsTrueAndValidMetaModelAndCustomerBelongsTo_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var validCustomerMetaModel = new TestValidCustomerIdentityMetaModel { CustId = 123, TransId = 456 };
            var actionArguments = new Dictionary<string, object> { { "model", validCustomerMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var nextCalled = false;
            var next = new ActionExecutionDelegate(async () => 
            {
                nextCalled = true;
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongsTo(agentList, 123, 456))
                .ReturnsAsync(true);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
            _mockLicenseeService.Verify(x => x.CheckCustomerBelongsTo(agentList, 123, 456), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdIsTrueAndValidMetaModelButCustomerDoesNotBelong_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var validCustomerMetaModel = new TestValidCustomerIdentityMetaModel { CustId = 123, TransId = 456 };
            var actionArguments = new Dictionary<string, object> { { "model", validCustomerMetaModel } };
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongsTo(agentList, 123, 456))
                .ReturnsAsync(false);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
            _mockLicenseeService.Verify(x => x.CheckCustomerBelongsTo(agentList, 123, 456), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenNoMetaModelAndValidateCustIdFalse_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: false
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments);
            var nextCalled = false;
            var next = new ActionExecutionDelegate(async () => 
            {
                nextCalled = true;
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenNoMetaModelAndValidateCustIdTrue_ShouldSetBadRequestResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                validateCustId: true
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments, isAjaxRequest: true);
            var next = CreateActionExecutionDelegate();

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.NotNull(context.Result);
            var jsonResult = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
        }

        #endregion

        #region Helper Methods

        private ActionExecutingContext CreateActionExecutingContext(IDictionary<string, object> actionArguments, bool isAjaxRequest = false)
        {
            var controller = new Mock<Controller>();
            var actionDescriptor = new Mock<ActionDescriptor>();
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();

            // Setup request headers to avoid null reference exception
            request.Setup(r => r.Headers).Returns(headers);
            request.Setup(r => r.Path).Returns(new PathString("/test"));
            
            // Setup AJAX header if needed
            if (isAjaxRequest)
            {
                headers["X-Requested-With"] = "XMLHttpRequest";
            }
            
            // Setup response headers
            response.Setup(r => r.Headers).Returns(new HeaderDictionary());
            
            httpContext.Setup(c => c.Request).Returns(request.Object);
            httpContext.Setup(c => c.Response).Returns(response.Object);

            return new ActionExecutingContext(
                new ActionContext(httpContext.Object, new Microsoft.AspNetCore.Routing.RouteData(), actionDescriptor.Object),
                new List<IFilterMetadata>(),
                actionArguments,
                controller.Object
            );
        }

        private ActionExecutionDelegate CreateActionExecutionDelegate()
        {
            return new ActionExecutionDelegate(async () => 
            {
                var context = new ActionExecutingContext(
                    new ActionContext(new Mock<HttpContext>().Object, new Microsoft.AspNetCore.Routing.RouteData(), new Mock<ActionDescriptor>().Object),
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    new Mock<Controller>().Object
                );
                return new ActionExecutedContext(context, new List<IFilterMetadata>(), context.Controller);
            });
        }

        #endregion

        #region Test Meta Models

        public class TestValidValidationMetaModel : IValidationMetaModel
        {
            public bool IsValid => true;
        }

        public class TestInvalidValidationMetaModel : IValidationMetaModel
        {
            public bool IsValid => false;
        }

        public class TestCustomerIdentityMetaModel : ICustomerIdentityMetaModel
        {
            public int CustId { get; set; }
            public long TransId { get; set; }
        }

        public class TestValidCustomerIdentityMetaModel : IValidationMetaModel, ICustomerIdentityMetaModel
        {
            public int CustId { get; set; }
            public long TransId { get; set; }
            public bool IsValid => true;
        }

        #endregion
    }
} 