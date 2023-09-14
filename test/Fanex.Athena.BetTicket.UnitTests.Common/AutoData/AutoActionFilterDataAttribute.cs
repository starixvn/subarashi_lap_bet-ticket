using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Fanex.Athena.BetTicket.UnitTests.Common.AutoData
{
    public class AutoActionFilterDataAttribute : AutoDataAttribute
    {
        public AutoActionFilterDataAttribute() :
            base(() =>
                 {
                     var fixture = new Fixture();

                     var modelState = new ModelStateDictionary();

                     var actionContext = new ActionContext(
                                                           new DefaultHttpContext(),
                                                           Mock.Of<RouteData>(),
                                                           Mock.Of<ActionDescriptor>(),
                                                           modelState
                                                          );

                     var actionExecutingContext = new ActionExecutingContext(
                                                                             actionContext,
                                                                             new List<IFilterMetadata>(),
                                                                             new Dictionary<string, object>(),
                                                                             Mock.Of<Controller>()
                                                                            );
                     fixture.Register(() => actionExecutingContext);
                     fixture.Customize(new AutoMoqCustomization());
                     return fixture;
                 }) { }
    }
}
