using Fanex.Athena.BetTicket.Core;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Repositories;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests
{
    public class DefendencyInjectionTests
    {
        [Fact]
        public void AddCore_WithValidServiceCollection_ShouldReturnServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddCore();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddCore_WithNullServiceCollection_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DefendencyInjection.AddCore(null));
        }

        [Fact]
        public void AddCore_ShouldRegisterBetListApiClientAsTransient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient(); // Register HttpClient for BetListApiClient

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<IBetListApiClient>();
            var service2 = serviceProvider.GetService<IBetListApiClient>();
            
            Assert.NotNull(service1);
            Assert.NotNull(service2);
            Assert.NotSame(service1, service2); // Transient services should be different instances
        }

        [Fact]
        public void AddCore_ShouldRegisterContextServiceAsScoped()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<IContextService>();
            var service2 = serviceProvider.GetService<IContextService>();
            
            Assert.NotNull(service1);
            Assert.NotNull(service2);
            Assert.Same(service1, service2); // Scoped services should be the same instance within the same scope
        }

        [Fact]
        public void AddCore_ShouldRegisterLicenseeServiceAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<ILicenseeService>();
            var service2 = serviceProvider.GetService<ILicenseeService>();
            
            Assert.NotNull(service1);
            Assert.NotNull(service2);
            Assert.Same(service1, service2); // Singleton services should be the same instance
        }



        [Fact]
        public void AddCore_ShouldRegisterHttpContextAccessor()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            Assert.NotNull(httpContextAccessor);
        }

        [Fact]
        public void AddCore_ShouldRegisterDbFactoryAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<IDbFactory>();
            var service2 = serviceProvider.GetService<IDbFactory>();
            
            Assert.NotNull(service1);
            Assert.NotNull(service2);
            Assert.Same(service1, service2); // Singleton services should be the same instance
        }

        [Fact]
        public void AddCore_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient(); // Register HttpClient for BetListApiClient

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetService<IBetListApiClient>());
            Assert.NotNull(serviceProvider.GetService<IContextService>());
            Assert.NotNull(serviceProvider.GetService<ILicenseeService>());
            Assert.NotNull(serviceProvider.GetService<IHttpContextAccessor>());
            Assert.NotNull(serviceProvider.GetService<IDbFactory>());
        }

        [Fact]
        public void AddCore_ShouldReturnSameServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddCore();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddCore_ShouldAllowMultipleCalls()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient(); // Register HttpClient for BetListApiClient

            // Act
            services.AddCore();
            services.AddCore(); // Should not throw

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider.GetService<IBetListApiClient>());
        }

        [Fact]
        public void AddCore_ShouldRegisterServicesWithCorrectTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient(); // Register HttpClient for BetListApiClient

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var betListApiClient = serviceProvider.GetService<IBetListApiClient>();
            var contextService = serviceProvider.GetService<IContextService>();
            var licenseeService = serviceProvider.GetService<ILicenseeService>();
            var dbFactory = serviceProvider.GetService<IDbFactory>();

            Assert.IsType<BetListApiClient>(betListApiClient);
            Assert.IsType<ContextService>(contextService);
            Assert.IsType<LicenseeService>(licenseeService);
            Assert.IsType<DbFactory>(dbFactory);
        }

        [Fact]
        public void AddCore_ShouldRegisterServicesWithCorrectLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient(); // Register HttpClient for BetListApiClient

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to test scoped services
            using var scope = serviceProvider.CreateScope();

            // Assert
            // Transient services should be different instances
            var transient1 = serviceProvider.GetService<IBetListApiClient>();
            var transient2 = serviceProvider.GetService<IBetListApiClient>();
            Assert.NotSame(transient1, transient2);

            // Scoped services should be the same within scope
            var scoped1 = scope.ServiceProvider.GetService<IContextService>();
            var scoped2 = scope.ServiceProvider.GetService<IContextService>();
            Assert.Same(scoped1, scoped2);

            // Singleton services should be the same across all scopes
            var singleton1 = serviceProvider.GetService<ILicenseeService>();
            var singleton2 = scope.ServiceProvider.GetService<ILicenseeService>();
            Assert.Same(singleton1, singleton2);
        }

        [Fact]
        public void AddCore_ShouldNotRegisterUnrelatedServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            // These services should not be registered
            Assert.Null(serviceProvider.GetService<string>());
        }
    }
} 