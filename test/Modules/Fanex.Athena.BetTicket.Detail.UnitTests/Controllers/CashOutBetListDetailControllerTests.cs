using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class CashOutBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        private readonly CashOutBetListDetailController _controller;

        public CashOutBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();
            _controller = new CashOutBetListDetailController();
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange & Act
            var authorizeAttribute = Attribute.GetCustomAttribute(typeof(CashOutBetListDetailController), typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            // Assert
            Assert.NotNull(authorizeAttribute);
            Assert.Equal("ViewDetail", authorizeAttribute.Policy);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange & Act
            var methodInfo = typeof(CashOutBetListDetailController).GetMethod("Index");
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute)) as HttpGetAttribute;

            // Assert
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Index_ShouldHaveAutoBindingMetaModelAttribute()
        {
            // Arrange & Act
            var methodInfo = typeof(CashOutBetListDetailController).GetMethod("Index");
            Assert.NotNull(methodInfo);
            var autoBindingAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(Fanex.Athena.BetTicket.Core.Attributes.AutoBindingMetaModelAttribute)) as Fanex.Athena.BetTicket.Core.Attributes.AutoBindingMetaModelAttribute;

            // Assert
            Assert.NotNull(autoBindingAttribute);
        }

        [Fact]
        public void Index_ShouldHaveResponseCacheAttribute()
        {
            // Arrange & Act
            var methodInfo = typeof(CashOutBetListDetailController).GetMethod("Index");
            Assert.NotNull(methodInfo);
            var responseCacheAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(ResponseCacheAttribute)) as ResponseCacheAttribute;

            // Assert
            Assert.NotNull(responseCacheAttribute);
            Assert.Equal(0, responseCacheAttribute.Duration);
            Assert.Equal(ResponseCacheLocation.None, responseCacheAttribute.Location);
            Assert.True(responseCacheAttribute.NoStore);
        }

        [Fact]
        public async Task Index_WithValidParameters_ShouldReturnViewResult()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel
            {
                TransId = 12345,
                RefNo = 67890,
                BetType = 1,
                BetdaqId = 11111,
                AgentListXml = "<agents></agents>",
                IsCashOut = true,
                IsShowIp = true,
                IsShowIpVN = false
            };

            var expectedViewModel = new CashOutBetListDetailViewModel
            {
                IsCashOut = true,
                IsHighlightOriginalTicket = false,
                CashOutTicketList = new List<CashOutTicket>(),
                CashOutTicketTextList = new List<CashOutTicketText>()
            };

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                "/api/CashOutBetList/Detail", 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                "en-US"))
                .ReturnsAsync(expectedViewModel);

            // Act
            var result = await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal(expectedViewModel, viewResult.Model);
        }

        [Fact]
        public async Task Index_ShouldSetHasPlayerComFromContextService()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedViewModel = new CashOutBetListDetailViewModel();

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.False(metaModel.HasPlayerCom);
            _mockContextService.Verify(x => x.IsAllowedCustomerComm, Times.Once);
        }

        [Fact]
        public async Task Index_ShouldCallBetListApiClientWithCorrectParameters()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel
            {
                TransId = 12345,
                RefNo = 67890,
                BetType = 1
            };

            var expectedViewModel = new CashOutBetListDetailViewModel();
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("th-TH");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<CashOutBetListDetailViewModel>(
                "/api/CashOutBetList/Detail", 
                metaModel, 
                "th-TH"), 
                Times.Once);
        }

        [Fact]
        public async Task Index_WithNullMetaModel_ShouldHandleGracefully()
        {
            // Arrange
            CashOutBetListDetailMetaModel metaModel = null;
            var expectedViewModel = new CashOutBetListDetailViewModel();

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel));
        }

        [Fact]
        public async Task Index_WithNullApiClient_ShouldThrowNullReferenceException()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.Index(null, _mockContextService.Object, metaModel));
        }

        [Fact]
        public async Task Index_WithNullContextService_ShouldThrowNullReferenceException()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.Index(_mockBetListApiClient.Object, null, metaModel));
        }

        [Fact]
        public async Task Index_ShouldSetMetaModelPropertiesCorrectly()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel
            {
                TransId = 12345,
                RefNo = 67890,
                BetType = 1,
                BetdaqId = 11111,
                AgentListXml = "<agents></agents>",
                IsCashOut = true,
                IsShowIp = true,
                IsShowIpVN = false
            };

            var expectedViewModel = new CashOutBetListDetailViewModel();
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(12345, metaModel.TransId);
            Assert.Equal(67890, metaModel.RefNo);
            Assert.Equal(1, metaModel.BetType);
            Assert.Equal(11111, metaModel.BetdaqId);
            Assert.Equal("<agents></agents>", metaModel.AgentListXml);
            Assert.True(metaModel.IsCashOut);
            Assert.True(metaModel.IsShowIp);
            Assert.False(metaModel.IsShowIpVN);
            Assert.True(metaModel.HasPlayerCom);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task Index_ShouldSetHasPlayerComBasedOnContextService(bool isAllowedCustomerComm, bool expectedHasPlayerCom)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedViewModel = new CashOutBetListDetailViewModel();

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(isAllowedCustomerComm);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(expectedHasPlayerCom, metaModel.HasPlayerCom);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("th-TH")]
        [InlineData("id-ID")]
        public async Task Index_ShouldPassCorrectLanguageToApiClient(string language)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedViewModel = new CashOutBetListDetailViewModel();

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns(language);
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            await _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<CashOutBetListDetailViewModel>(
                "/api/CashOutBetList/Detail", 
                metaModel, 
                language), 
                Times.Once);
        }

        [Fact]
        public async Task Index_WhenApiClientThrowsException_ShouldPropagateException()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedException = new InvalidOperationException("API Error");

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en-US");
            _mockBetListApiClient.Setup(x => x.Post<CashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<CashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel));
            Assert.Equal("API Error", exception.Message);
        }

        [Fact]
        public void Controller_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new CashOutBetListDetailController();

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Controller_ShouldBePublic()
        {
            // Arrange & Act
            var controllerType = typeof(CashOutBetListDetailController);

            // Assert
            Assert.True(controllerType.IsPublic);
        }
    }
} 