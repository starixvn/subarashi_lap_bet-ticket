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

namespace Fanex.Athena.BetTicket.Core.UnitTests
{
    public class AutoBindingMetaModelAttributeTests
    {
        private readonly Mock<IMetaModelBindingService> _mockBindingService;
        private readonly Mock<IContextService> _mockContextService;

        public AutoBindingMetaModelAttributeTests()
        {
            _mockBindingService = new Mock<IMetaModelBindingService>();
            _mockContextService = new Mock<IContextService>();
        }

        #region AutoBindingMetaModelAttribute Tests

        [Fact]
        public void Constructor_WithDefaultParameters_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new AutoBindingMetaModelAttribute();

            // Assert
            Assert.Equal(typeof(AutoBindingMetaModelImplAttribute), attribute.ImplementationType);
            Assert.Equal(5, attribute.Arguments.Length);
            Assert.False((bool)attribute.Arguments[0]); // bindAgent
            Assert.False((bool)attribute.Arguments[1]); // bindLanguage
            Assert.False((bool)attribute.Arguments[2]); // bindLicenseeId
            Assert.False((bool)attribute.Arguments[3]); // bindPlayerComm
            Assert.False((bool)attribute.Arguments[4]); // bindBetFromIP
        }

        [Fact]
        public void Constructor_WithAllParametersTrue_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new AutoBindingMetaModelAttribute(
                bindAgent: true,
                bindLanguage: true,
                bindLicenseeId: true,
                bindPlayerComm: true,
                bindBetFromIP: true
            );

            // Assert
            Assert.Equal(typeof(AutoBindingMetaModelImplAttribute), attribute.ImplementationType);
            Assert.Equal(5, attribute.Arguments.Length);
            Assert.True((bool)attribute.Arguments[0]); // bindAgent
            Assert.True((bool)attribute.Arguments[1]); // bindLanguage
            Assert.True((bool)attribute.Arguments[2]); // bindLicenseeId
            Assert.True((bool)attribute.Arguments[3]); // bindPlayerComm
            Assert.True((bool)attribute.Arguments[4]); // bindBetFromIP
        }

        [Fact]
        public void Constructor_WithMixedParameters_ShouldSetCorrectArguments()
        {
            // Act
            var attribute = new AutoBindingMetaModelAttribute(
                bindAgent: true,
                bindLanguage: false,
                bindLicenseeId: true,
                bindPlayerComm: false,
                bindBetFromIP: true
            );

            // Assert
            Assert.Equal(typeof(AutoBindingMetaModelImplAttribute), attribute.ImplementationType);
            Assert.Equal(5, attribute.Arguments.Length);
            Assert.True((bool)attribute.Arguments[0]); // bindAgent
            Assert.False((bool)attribute.Arguments[1]); // bindLanguage
            Assert.True((bool)attribute.Arguments[2]); // bindLicenseeId
            Assert.False((bool)attribute.Arguments[3]); // bindPlayerComm
            Assert.True((bool)attribute.Arguments[4]); // bindBetFromIP
        }

        #endregion

        #region AutoBindingMetaModelImplAttribute Tests

        [Fact]
        public void Constructor_ShouldSetAllProperties()
        {
            // Arrange
            var bindAgent = true;
            var bindLanguage = false;
            var bindLicenseeId = true;
            var bindPlayerComm = false;
            var bindBetFromIP = true;

            // Act
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent,
                bindLanguage,
                bindLicenseeId,
                bindPlayerComm,
                bindBetFromIP
            );

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindAgentIsTrue_ShouldCallBindAgentXml()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: true,
                bindLanguage: false,
                bindLicenseeId: false,
                bindPlayerComm: false,
                bindBetFromIP: false
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            _mockBindingService.Verify(x => x.BindAgentXml(actionArguments, agentList), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindAgentIsFalse_ShouldNotCallBindAgentXml()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: false,
                bindLicenseeId: false,
                bindPlayerComm: false,
                bindBetFromIP: false
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            _mockBindingService.Verify(x => x.BindAgentXml(It.IsAny<IDictionary<string, object>>(), It.IsAny<IDictionary<int, int[]>>()), Times.Never);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindPlayerCommIsTrue_ShouldCallBindPlayerCom()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: false,
                bindLicenseeId: false,
                bindPlayerComm: true,
                bindBetFromIP: false
            );

            var actionArguments = new Dictionary<string, object>();
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            _mockBindingService.Verify(x => x.BindPlayerCom(actionArguments, true), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindLicenseeIdIsTrueAndMetaModelExists_ShouldSetLicenseeId()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: false,
                bindLicenseeId: true,
                bindPlayerComm: false,
                bindBetFromIP: false
            );

            var licenseeIdMetaModel = new TestLicenseeIdMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", licenseeIdMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            _mockContextService.Setup(x => x.LicenseeId).Returns(123);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.Equal(123, licenseeIdMetaModel.LicenseeId);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindBetFromIPIsTrueAndMetaModelExists_ShouldSetShowIpProperties()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: false,
                bindLicenseeId: false,
                bindPlayerComm: false,
                bindBetFromIP: true
            );

            var showBetFromIPMetaModel = new TestShowBetFromIPMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", showBetFromIPMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            _mockContextService.Setup(x => x.IsShowIP).Returns(true);
            _mockContextService.Setup(x => x.IsShowVNIP).Returns(false);

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(showBetFromIPMetaModel.IsShowIp);
            Assert.False(showBetFromIPMetaModel.IsShowIpVN);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindLanguageIsTrueAndMetaModelExists_ShouldSetLanguage()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: true,
                bindLicenseeId: false,
                bindPlayerComm: false,
                bindBetFromIP: false
            );

            var languageMetaModel = new TestLanguageMetaModel();
            var actionArguments = new Dictionary<string, object> { { "model", languageMetaModel } };
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            _mockContextService.Setup(x => x.Language).Returns("en-US");

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.Equal("en-US", languageMetaModel.Language);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenAllBindingsAreTrue_ShouldCallAllServices()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: true,
                bindLanguage: true,
                bindLicenseeId: true,
                bindPlayerComm: true,
                bindBetFromIP: true
            );

            var licenseeIdMetaModel = new TestLicenseeIdMetaModel();
            var showBetFromIPMetaModel = new TestShowBetFromIPMetaModel();
            var languageMetaModel = new TestLanguageMetaModel();
            var actionArguments = new Dictionary<string, object> 
            { 
                { "licenseeModel", licenseeIdMetaModel },
                { "showIpModel", showBetFromIPMetaModel },
                { "languageModel", languageMetaModel }
            };
            var context = CreateActionExecutingContext(actionArguments);
            var next = CreateActionExecutionDelegate();

            var agentList = new Dictionary<int, int[]>();
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.LicenseeId).Returns(456);
            _mockContextService.Setup(x => x.IsShowIP).Returns(true);
            _mockContextService.Setup(x => x.IsShowVNIP).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("th-TH");

            // Act
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            _mockBindingService.Verify(x => x.BindAgentXml(actionArguments, agentList), Times.Once);
            _mockBindingService.Verify(x => x.BindPlayerCom(actionArguments, true), Times.Once);
            Assert.Equal(456, licenseeIdMetaModel.LicenseeId);
            Assert.True(showBetFromIPMetaModel.IsShowIp);
            Assert.True(showBetFromIPMetaModel.IsShowIpVN);
            Assert.Equal("th-TH", languageMetaModel.Language);
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldCallNextDelegate()
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockContextService.Object,
                bindAgent: false,
                bindLanguage: false,
                bindLicenseeId: false,
                bindPlayerComm: false,
                bindBetFromIP: false
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
        }

        #endregion

        #region Helper Methods

        private ActionExecutingContext CreateActionExecutingContext(IDictionary<string, object> actionArguments)
        {
            var controller = new Mock<Controller>();
            var actionDescriptor = new Mock<ActionDescriptor>();
            var httpContext = new Mock<HttpContext>();

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

        public class TestLicenseeIdMetaModel : ILicenseeIdMetaModel
        {
            public int LicenseeId { get; set; }
        }

        public class TestShowBetFromIPMetaModel : IShowBetFromIPMetaModel
        {
            public bool IsShowIp { get; set; }
            public bool IsShowIpVN { get; set; }
        }

        public class TestLanguageMetaModel : ILanguageMetaModel
        {
            public string Language { get; set; }
        }

        #endregion
    }
} 