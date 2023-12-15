using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Attributes
{
    public class AutoBindingMetaModelAttributeTest
    {
        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindAgentTrue_CallsBindingService(
            Mock<IMetaModelBindingService> bindingService,
            Mock<IContextService> contextService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  true,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  false
                                                                 );
            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            bindingService
               .Verify(mock => mock.BindAgentXml(It.IsAny<IDictionary<string, object>>(), It.IsAny<IDictionary<int, int[]>>()),
                       Times.Once);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindLanguageTrue_BindLanguage(
            Mock<ILanguageMetaModel> languageMetaModel,
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            const string expectedLanguage = "Any";
            contextService.Setup(x => x.Language).Returns(expectedLanguage);
            actionContext.ActionArguments["languageMetaModel"] = languageMetaModel.Object;

            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  true,
                                                                  false,
                                                                  false,
                                                                  false
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.Language, Times.Once);
            languageMetaModel.VerifySet(x => x.Language = expectedLanguage, Times.Once);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindLanguageTrueWithoutMetaModel_DoNothing(
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  true,
                                                                  false,
                                                                  false,
                                                                  false
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.Language, Times.Never);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindLicenseeIdTrue_BindLicenseeId(
            Mock<ILicenseeIdMetaModel> licenseeIdMetaModel,
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            const int expectedId = 10;
            contextService.Setup(x => x.LicenseeId).Returns(expectedId);
            actionContext.ActionArguments["licenseeIdMetaModel"] = licenseeIdMetaModel.Object;

            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  false,
                                                                  true,
                                                                  false,
                                                                  false
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.LicenseeId, Times.Once);
            licenseeIdMetaModel.VerifySet(x => x.LicenseeId = expectedId, Times.Once);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindLicenseeIdTrueWithoutMetaModel_DoNothing(
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  false,
                                                                  true,
                                                                  false,
                                                                  false
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.LicenseeId, Times.Never);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindPlayerCommTrue_CallsBindingService(
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  true,
                                                                  false
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            bindingService.Verify(mock => mock.BindPlayerCom(It.IsAny<IDictionary<string, object>>(), It.IsAny<bool>()),
                                  Times.Once);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindBetFromIPTrue_BindBetFromIP(
            Mock<IShowBetFromIPMetaModel> showBetFromIpMetaModel,
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            const bool expectedIsShowIp = false;
            const bool expectedIsShowVnIp = true;
            contextService.Setup(x => x.IsShowIP).Returns(expectedIsShowIp);
            contextService.Setup(x => x.IsShowVNIP).Returns(expectedIsShowVnIp);
            actionContext.ActionArguments["showBetFromIPMetaModel"] = showBetFromIpMetaModel.Object;

            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  true
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.IsShowIP, Times.Once);
            contextService.VerifyGet(cs => cs.IsShowVNIP, Times.Once);
            showBetFromIpMetaModel.VerifySet(x => x.IsShowIp = expectedIsShowIp, Times.Once);
            showBetFromIpMetaModel.VerifySet(x => x.IsShowIpVN = expectedIsShowVnIp, Times.Once);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_BindBetFromIPTrueWithoutMetaModel_DoNothing(
            [Frozen] Mock<IContextService> contextService,
            Mock<IMetaModelBindingService> bindingService,
            ActionExecutingContext actionContext)
        {
            // Arrange
            var attribute = new AutoBindingMetaModelImplAttribute(
                                                                  bindingService.Object,
                                                                  contextService.Object,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  false,
                                                                  true
                                                                 );

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionContext, next);

            // Assert
            contextService.VerifyGet(cs => cs.IsShowIP, Times.Never);
            contextService.VerifyGet(cs => cs.IsShowVNIP, Times.Never);
        }
    }
}
