using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Fanex.Athena.BetTicket.UnitTests.Common.AutoData
{
    public class AutoContextDataAttribute : AutoDataAttribute
    {
        public AutoContextDataAttribute() :
            base(() =>
                 {
                     var fixture = new Fixture();

                     RegisterHttpContext(fixture);

                     fixture.Customize(new AutoMoqCustomization());
                     return fixture;
                 }) { }

        private static void RegisterHttpContext(IFixture fixture)
        {
            var context = new DefaultHttpContext
            {
                Request =
                {
                    RouteValues = new RouteValueDictionary(new[]
                    {
                        new KeyValuePair<string, object>("controller", "Any")
                    })
                }
            };
            fixture.Register<HttpContext>(() => context);
        }
    }
}
