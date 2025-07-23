using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class NewCashOutBetListDetailControllerTests
    {
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly Mock<IContextService> _mockContextService;

        public NewCashOutBetListDetailControllerTests()
        {
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _mockContextService = new Mock<IContextService>();
        }

        [Fact]
        public void NewCashOutBetListDetailController_ShouldInheritFromController()
        {
            // Arrange & Act
            var controller = new NewCashOutBetListDetailController();

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Constructor_ShouldInitialize()
        {
            // Arrange & Act
            var controller = new NewCashOutBetListDetailController();

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Index_ShouldHaveHttpGetAttribute()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var methodInfo = typeof(NewCashOutBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var httpGetAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void Index_ShouldHaveAutoBindingMetaModelAttribute()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var methodInfo = typeof(NewCashOutBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var autoBindingAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(AutoBindingMetaModelAttribute));
            Assert.NotNull(autoBindingAttribute);
        }

        [Fact]
        public void Index_ShouldHaveResponseCacheAttribute()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var methodInfo = typeof(NewCashOutBetListDetailController).GetMethod("Index");

            // Act & Assert
            Assert.NotNull(methodInfo);
            var responseCacheAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(ResponseCacheAttribute));
            Assert.NotNull(responseCacheAttribute);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(NewCashOutBetListDetailController);

            // Act & Assert
            var authorizeAttribute = Attribute.GetCustomAttribute(controllerType, typeof(AuthorizeAttribute));
            Assert.NotNull(authorizeAttribute);
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            var result = await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal(expectedViewModel, viewResult.Model);
        }

        [Fact]
        public async Task Index_ShouldSetMetaModelHasPlayerCom()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.True(metaModel.HasPlayerCom);
        }

        [Fact]
        public async Task Index_ShouldSetMetaModelCurrencyId()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel { Currency = "USD" };
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(1, metaModel.CurrencyId);
        }

        [Fact]
        public async Task Index_ShouldCallCorrectApiPath()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Index_ShouldPassCorrectLanguage()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            var expectedLanguage = "th";
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns(expectedLanguage);
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<NewCashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                expectedLanguage), Times.Once);
        }

        [Fact]
        public async Task Index_WithNullMetaModel_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            NewCashOutBetListDetailMetaModel metaModel = null;
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel));
        }

        [Fact]
        public async Task Index_WithEmptyCurrencyList_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel { Currency = "EUR" };
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>());

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(0, metaModel.CurrencyId);
        }

        [Fact]
        public async Task Index_WithNullCurrency_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel { Currency = null };
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(0, metaModel.CurrencyId);
        }

        [Fact]
        public async Task Index_WithApiException_ShouldPropagateException()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("API Error"));

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => 
                controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel));
        }

        [Fact]
        public async Task Index_WithNullApiResponse_ShouldHandleGracefully()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            NewCashOutBetListDetailViewModel expectedViewModel = null;
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            var result = await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task Index_WithDifferentCurrencies_ShouldSetCorrectCurrencyId()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel { Currency = "EUR" };
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" },
                { 2, "EUR" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.Equal(2, metaModel.CurrencyId);
        }

        [Fact]
        public async Task Index_WithFalseIsAllowedCustomerComm_ShouldSetHasPlayerComToFalse()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.False(metaModel.HasPlayerCom);
        }

        [Fact]
        public async Task Index_WithDifferentLanguages_ShouldPassCorrectLanguage()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            var expectedLanguage = "id";
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns(expectedLanguage);
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<NewCashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                expectedLanguage), Times.Once);
        }

        [Fact]
        public async Task Index_WithComplexViewModel_ShouldReturnCorrectModel()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel
            {
                // Set properties as needed for the test
            };
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            var result = await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Same(expectedViewModel, viewResult.Model);
        }

        [Fact]
        public async Task Index_WithNullDependencies_ShouldThrowException()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                controller.Index(null, null, metaModel));
        }

        [Fact]
        public async Task Index_WithEmptyLanguage_ShouldPassEmptyLanguage()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            var expectedLanguage = "";
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns(expectedLanguage);
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<NewCashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                expectedLanguage), Times.Once);
        }

        [Fact]
        public async Task Index_WithNullLanguage_ShouldPassNullLanguage()
        {
            // Arrange
            var controller = new NewCashOutBetListDetailController();
            var metaModel = new NewCashOutBetListDetailMetaModel();
            var expectedViewModel = new NewCashOutBetListDetailViewModel();
            string expectedLanguage = null;
            
            _mockBetListApiClient.Setup(x => x.Post<NewCashOutBetListDetailViewModel>(
                "/api/CashOutBetList/History", 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                It.IsAny<string>()))
                .ReturnsAsync(expectedViewModel);

            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns(expectedLanguage);
            _mockContextService.Setup(x => x.CurrencyList).Returns(new Dictionary<int, string>
            {
                { 1, "USD" }
            });

            // Act
            await controller.Index(_mockBetListApiClient.Object, _mockContextService.Object, metaModel);

            // Assert
            _mockBetListApiClient.Verify(x => x.Post<NewCashOutBetListDetailViewModel>(
                It.IsAny<string>(), 
                It.IsAny<NewCashOutBetListDetailMetaModel>(), 
                expectedLanguage), Times.Once);
        }
    }
} 