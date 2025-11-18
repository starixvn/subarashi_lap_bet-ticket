using Fanex.Athena.BetTicket.Core.Services;
using Fanex.RemotingRef;
using Moq;
using System.Globalization;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Services
{
    public class ResourceServiceTests
    {
        private readonly Mock<IRemotingRefService> _mockRemotingRefService;
        private readonly ResourceService _resourceService;

        public ResourceServiceTests()
        {
            _mockRemotingRefService = new Mock<IRemotingRefService>();
            _resourceService = new ResourceService(_mockRemotingRefService.Object);
        }

        [Fact]
        public void GetSportName_WithValidSportId_ShouldReturnSportName()
        {
            var sportId = 1;
            var languageCode = "en-US";
            var expectedSportName = "Football";

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), languageCode, string.Empty))
                .Returns(expectedSportName);

            var result = _resourceService.GetSportName(sportId, languageCode);

            Assert.Equal(expectedSportName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), languageCode, string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_WithValidSportIdAsString_ShouldReturnSportName()
        {
            var sportId = "2";
            var languageCode = "th-TH";
            var expectedSportName = "Basketball";

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId, languageCode, string.Empty))
                .Returns(expectedSportName);

            var result = _resourceService.GetSportName(sportId, languageCode);

            Assert.Equal(expectedSportName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId, languageCode, string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_WithNullLanguageCode_ShouldUseCurrentUICulture()
        {
            var sportId = 3;
            var expectedSportName = "Tennis";
            var currentCulture = CultureInfo.CurrentUICulture.Name;

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty))
                .Returns(expectedSportName);

            var result = _resourceService.GetSportName(sportId, null);

            Assert.Equal(expectedSportName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_WithEmptyLanguageCode_ShouldUseCurrentUICulture()
        {
            var sportId = 4;
            var expectedSportName = "Soccer";
            var currentCulture = CultureInfo.CurrentUICulture.Name;

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty))
                .Returns(expectedSportName);

            var result = _resourceService.GetSportName(sportId, string.Empty);

            Assert.Equal(expectedSportName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_WithNullSportId_ShouldReturnEmptyString()
        {
            var result = _resourceService.GetSportName(null, "en-US");

            Assert.Equal(string.Empty, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetSportName_WithEmptyStringSportId_ShouldReturnEmptyString()
        {
            var result = _resourceService.GetSportName(string.Empty, "en-US");

            Assert.Equal(string.Empty, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetSportName_WithZeroSportId_ShouldReturnEmptyString()
        {
            var result = _resourceService.GetSportName(0, "en-US");

            Assert.Equal(string.Empty, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetSportName_WithZeroAsStringSportId_ShouldReturnEmptyString()
        {
            var result = _resourceService.GetSportName("0", "en-US");

            Assert.Equal(string.Empty, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetSportName_WithValidSportIdAndNoLanguageCode_ShouldCallRemotingRefServiceWithCurrentCulture()
        {
            var sportId = 5;
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            var expectedSportName = "Baseball";

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty))
                .Returns(expectedSportName);

            var result = _resourceService.GetSportName(sportId);

            Assert.Equal(expectedSportName, result);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), currentCulture, string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_WithDifferentLanguageCodes_ShouldPassCorrectLanguageCode()
        {
            var sportId = 6;
            var languageCode1 = "zh-CN";
            var languageCode2 = "ja-JP";
            var expectedSportName1 = "Football";
            var expectedSportName2 = "バスケットボール";

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), languageCode1, string.Empty))
                .Returns(expectedSportName1);

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), languageCode2, string.Empty))
                .Returns(expectedSportName2);

            var result1 = _resourceService.GetSportName(sportId, languageCode1);
            var result2 = _resourceService.GetSportName(sportId, languageCode2);

            Assert.Equal(expectedSportName1, result1);
            Assert.Equal(expectedSportName2, result2);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), languageCode1, string.Empty), Times.Once);
            _mockRemotingRefService.Verify(x => x.GetSportName(sportId.ToString(), languageCode2, string.Empty), Times.Once);
        }
    }
}

