using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Fanex.Core.Utils;
using Fanex.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Attributes
{
    public class DataValidationAttributeTest
    {
        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_ValidArguments_InvokeNext(
            ActionExecutingContext actionExecutingContext,
            Mock<IContextService> contextService,
            Mock<ILicenseeService> licenseeService,
            Mock<IValidationMetaModel> validationMetaModel)
        {
            // Arrange
            licenseeService
               .Setup(x => x.CheckCustomerBelongsTo(It.IsAny<IReadOnlyDictionary<int, int[]>>(), It.IsAny<int>(),
                                                    It.IsAny<long>())).Returns(Task.FromResult(true));

            var attribute = new DataValidationImplAttribute(contextService.Object, licenseeService.Object, false);

            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));
            actionExecutingContext.ActionArguments["Any"] = validationMetaModel;

            // Act
            await attribute.OnActionExecutionAsync(actionExecutingContext, next);

            // Assert
            Assert.IsNotType<RedirectResult>(actionExecutingContext.Result);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_InValidArguments_RedirectInvalidRequest(
            ActionExecutingContext actionExecutingContext,
            Mock<IContextService> contextService,
            Mock<ILicenseeService> licenseeService)
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(contextService.Object, licenseeService.Object, true);
            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionExecutingContext, next);

            // Assert
            Assert.IsType<RedirectResult>(actionExecutingContext.Result);
        }

        [Theory] [AutoActionFilterData]
        public async Task OnActionExecutionAsync_InValidAjaxArguments_HandleBadRequest(
            ActionExecutingContext actionExecutingContext,
            Mock<IContextService> contextService,
            Mock<ILicenseeService> licenseeService)
        {
            // Arrange
            actionExecutingContext.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

            var attribute = new DataValidationImplAttribute(contextService.Object, licenseeService.Object, true);
            var next = new ActionExecutionDelegate(() => Task.FromResult<ActionExecutedContext>(null));

            // Act
            await attribute.OnActionExecutionAsync(actionExecutingContext, next);
            var statusCodeProperty = actionExecutingContext.Result!.GetType().GetProperty("StatusCode");

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest,
                         actionExecutingContext.Result.GetPropertyValue(statusCodeProperty));
        }
    }
}
