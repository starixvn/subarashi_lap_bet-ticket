using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Xunit;
using static System.String;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Services
{
    public class ContextServiceTest
    {
        [Fact]
        public async Task AccessToken_WhenCall_ReturnValue()
        {
            // Arrange
            var expectedToken = "Any";

            var serviceProvider = new Mock<IServiceProvider>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var authResult = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), Empty));

            authResult.Properties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = expectedToken }
            });

            authenticationServiceMock
               .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), null))
               .ReturnsAsync(authResult);

            serviceProvider.Setup(_ => _.GetService(typeof(IAuthenticationService)))
                           .Returns(authenticationServiceMock.Object);

            var httpAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider.Object
                }
            };

            var contextService = new ContextService(httpAccessor);

            // Act
            var result = await contextService.AccessToken;

            // Assert
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public void ClientIP_WhenCall_ReturnValue()
        {
            // Arrange
            var expected = FakeHelper.Create<IPAddress>();
            var httpAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    Connection = { RemoteIpAddress = expected }
                }
            };

            var contextService = new ContextService(httpAccessor);

            // Act
            var result = contextService.ClientIP;

            // Assert
            Assert.Equal(expected.ToString(), result);
        }

        [Fact]
        public void Language_WhenCall_ReturnValue()
        {
            // Arrange
            var expected = FakeHelper.Create<IPAddress>();
            var httpAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    Connection = { RemoteIpAddress = expected }
                }
            };

            var contextService = new ContextService(httpAccessor);

            // Act
            var result = contextService.ClientIP;

            // Assert
            Assert.Equal(expected.ToString(), result);
        }

        [Fact]
        public void LicenseeId_WhenCall_ReturnValue()
        {
            const string type = "LicenseeId";
            var expectedClaimValue = FakeHelper.Create<int>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.LicenseeId;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void IsAllowedCustomerComm_WhenCall_ReturnValue()
        {
            const string type = "IsAllowedCustomerComm";
            var expectedClaimValue = FakeHelper.Create<bool>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.IsAllowedCustomerComm;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void IsEnableBA_WhenCall_ReturnValue()
        {
            const string type = "IsEnableBA";
            var expectedClaimValue = FakeHelper.Create<bool>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.IsEnableBA;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void UserLevel_WhenCall_ReturnValue()
        {
            const string type = "UserLevel";
            var expectedClaimValue = FakeHelper.Create<int>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.UserLevel;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void SiteName_WhenCall_ReturnValue()
        {
            const string type = "SiteName";
            var expectedClaimValue = FakeHelper.Create<string>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.SiteName;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void Username_WhenCall_ReturnValue()
        {
            const string type = "Username";
            var expectedClaimValue = FakeHelper.Create<string>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.Username;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void UserId_WhenCall_ReturnValue()
        {
            const string type = "UserSEQ";
            var expectedClaimValue = FakeHelper.Create<int>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.UserId;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void IsShowIP_WhenCall_ReturnValue()
        {
            const string type = "IsShowIP";
            var expectedClaimValue = FakeHelper.Create<bool>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.IsShowIP;

            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void IsShowVnIP_IsShowIpFalse_ReturnFalse()
        {
            const string type = "IsShowIP";
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, false));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.IsShowVNIP;

            Assert.False(result);
        }

        [Fact]
        public void IsShowVnIP_NotInRole_ReturnFalse()
        {
            const string type = "IsShowIP";
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, true));
            var contextService = new ContextService(httpAccessor);

            var result = contextService.IsShowVNIP;

            Assert.False(result);
        }

        [Fact]
        public void IsShowVnIP_ShowIpWithInRole_ReturnTrue()
        {
            // Arrange
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithRoles("super_admin", "bl_view_ipvn"),
                                                                 ClaimsWithValue("IsShowIP", true));

            var contextService = new ContextService(httpAccessor);

            // Act
            var result = contextService.IsShowVNIP;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SiteDBServer_WhenCall_ReturnValue()
        {
            // Arrange
            const string type = "SiteDBServer";
            var expectedClaimValue = FakeHelper.Create<string>();
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithValue(type, expectedClaimValue));
            var contextService = new ContextService(httpAccessor);

            // Act
            var result = contextService.SiteDBServer;

            // Assert
            Assert.Equal(expectedClaimValue, result);
        }

        [Fact]
        public void SiteDBServer_NullValue_ReturnStringEmpty()
        {
            // Arrange
            var httpAccessor = FakeHttpContextAccessorWithClaims();
            var contextService = new ContextService(httpAccessor);

            // Act
            var result = contextService.SiteDBServer;

            // Assert
            Assert.Equal(Empty, result);
        }

        [Fact]
        public void CurrencyList_WhenCall_ReturnValue()
        {
            // Arrange
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithAgentList());

            // Act
            var contextService = new ContextService(httpAccessor);

            // Assert
            Assert.NotEmpty(contextService.CurrencyList);
        }

        [Fact]
        public void AgentList_WhenCall_ReturnValue()
        {
            // Arrange
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithAgentList());

            // Act
            var contextService = new ContextService(httpAccessor);

            // Assert
            Assert.NotEmpty(contextService.AgentList);
        }

        [Fact]
        public void IsShowIP_InValid_ThrowException()
        {
            // Arrange
            var httpAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new List<ClaimsIdentity>())
                }
            };

            // Act
            var contextService = new ContextService(httpAccessor);

            // Assert
            Assert.ThrowsAny<Exception>(() => contextService.IsShowIP);
        }

        [Fact]
        public void IsInRoles_InRole_ReturnTrue()
        {
            const string FakeRole = "FakeRole";
            var httpAccessor = FakeHttpContextAccessorWithClaims(ClaimsWithRoles(FakeRole));

            var contextService = new ContextService(httpAccessor);

            Assert.True(contextService.IsInRoles(FakeRole));
        }

        [Fact]
        public void IsInRoles_NotInRole_ReturnFalse()
        {
            var httpAccessor = FakeHttpContextAccessorWithClaims();

            var contextService = new ContextService(httpAccessor);

            Assert.False(contextService.IsInRoles("Any"));
        }

        [Fact]
        public void IsInRoles_EmptyInput_ReturnFalse()
        {
            var httpAccessor = FakeHttpContextAccessorWithClaims();

            var contextService = new ContextService(httpAccessor);

            Assert.False(contextService.IsInRoles());
        }

        private static HttpContextAccessor FakeHttpContextAccessorWithClaims(params IEnumerable<Claim>[] claims)
        {
            var claimIdentity = claims.AsEnumerable().Select(claim => new ClaimsIdentity(claim)).ToList();

            var httpAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claimIdentity)
                }
            };

            return httpAccessor;
        }

        private static IEnumerable<Claim> ClaimsWithAgentList()
        {
            var currencyAgents = FakeHelper.CreateMany<ContextService.CurrencyAgentInfo>();

            var currencyPrincipals = currencyAgents.Select(currencyAgentInfo =>
                                                               new Claim("CurrencyAgent",
                                                                         JsonConvert
                                                                            .SerializeObject(currencyAgentInfo)));
            return currencyPrincipals;
        }

        private static IEnumerable<Claim> ClaimsWithValue<T>(string type, T value)
        {
            var claim = new Claim(type, value.ToString());
            return new List<Claim> { claim };
        }

        private static IEnumerable<Claim> ClaimsWithRoles(params string[] roles)
        {
            return roles.Select(role => new Claim(ClaimTypes.Role, role));
        }
    }
}
